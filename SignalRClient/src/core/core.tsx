var adapter = require("webrtc-adapter");
import {config} from './../../config'
import axios from 'axios'
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';


export default class Core{
    private static instance: Core;
    private hubConnection: HubConnection;
    private token: string;


    private constructor(token: string){
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(config.SIGNALR + "?access_token=" + token)
            .configureLogging(0)
            .build();
        this.hubConnection.on("IncomingMessage", this.onChatMessage);
        this.hubConnection.start();

    }

    public static getInstance(){
        return Core.instance;
    }

    public SendMessage(text: string, groupGuid: string){
        return this.invoke("SendChatMessage",groupGuid, text)
    }

    public static login(login: string, password: string){
        return new Promise((resolve, reject)=>{
            Core.post(config.ROUTES.LOGIN, {login: login, password: password})
                .then((response: any)=>{
                      this.instance = new Core(response.data);
                      this.instance.token = response.data;
                      resolve();
                })
                .catch(er=> reject(er));
        }) 
    }

    private onChatMessage(message: any){
        console.warn("IncomingMessage: ", message)
    }

    private invoke(method: string, ...params:any[]){
        return new Promise((resolve, reject) =>{
            this.hubConnection.invoke(method, ...params)
                .then((res: any) =>{
                    if(res.error)
                        reject(res.error)
                    else
                        resolve(res)
                })
                .catch((er:any) => reject(er))
        })
    }    

    private static get(subdomain: string, params?: any){
        var link = `${config.REST}${subdomain}?`;
        for (let key in params){
            if(params.hasOwnProperty(key)){
                link += `${key}=${params[key]}&`;
            }
        }
        return new Promise((resolve, reject)=>{
            if(this.instance)
                var token = this.instance.token || "";
            axios.get(link, {withCredentials: true, headers:{"Authorization":"Bearer " + token}})
                .then((data: any)=> resolve(data))
                .catch(er=>{
                    if(!er.response || !er.response.data){
                        reject(er.message)
                        return
                    }
                    reject(er.response.data)
                })
        });
    }

    private static post(subdomain: string, params: object){
        return new Promise((resolve, reject)=>{
            var token = "";
            if(this.instance)
                var token = this.instance.token || "";
            axios.post(`${config.REST}${subdomain}`, params, {withCredentials: false, headers:{"Authorization":"Bearer " + token}})
                .then(resolve)
                .catch(er=>{
                    if(!er.response || !er.response.data){
                        reject(er.message)
                        return
                    }
                    reject(er.response.data)
                })
        })
    }   


}