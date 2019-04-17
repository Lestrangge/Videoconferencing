var adapter = require("webrtc-adapter");
import {config} from '../../config'
import axios from 'axios'
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import WebrtcStuff from "./webrtcStuff"
import RemoteWebrtcStuff from './remoteWebrtcStuff';

export default class Core{
    private static instance: Core;
    private hubConnection: HubConnection;
    private token: string;
    private webRtcStuff: WebrtcStuff;
    private remoteWebRtcStuff : RemoteWebrtcStuff;

    private constructor(token: string){
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(config.SIGNALR + "?access_token=" + token)
            .configureLogging(0)
            .build();
        this.webRtcStuff = new WebrtcStuff(this.trickle)
        this.remoteWebRtcStuff = new RemoteWebrtcStuff(this.trickle)
        this.hubConnection.on("IncomingMessage", this.onChatMessage);
        this.hubConnection.on("NewPublisher", this.onNewPublisher);

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

    public initCall(onLocalStream: (stream: MediaStream) => void, 
                onRemoteStream: (stream: MediaStream) => void,
                groupGuid: string){
        var that = this;
        return new Promise((resolve, reject)=> {
            that.webRtcStuff.setOnLocalStream(onLocalStream);
            that.remoteWebRtcStuff.setOnRemoteStream(onRemoteStream);
            that.webRtcStuff.generateSdp()
                .then((jsep: any)=>{
                    console.log("Jsep: ", jsep);
                    that.invoke("InitiateCall", {'offer': jsep, "groupGuid": groupGuid})
                        .then((response:any)=>{
                            this.webRtcStuff.handleAnswer(response.data)
                        })
                })
                .catch(reject)
        })
    }

    private trickle(candidate: any){
        Core.getInstance().invoke("Trickle", candidate)
    }

    private onChatMessage(message: any){
        console.warn("IncomingMessage: ", message)
    }

    private onNewPublisher(response: any){
        var that = Core.getInstance();
        that.remoteWebRtcStuff.generateSdp(response.jsep)
            .then((answer: any)=>{
                that.invoke("AnswerNewPublisher", {'answer': answer, "handleId":response.handleId})
                    .then((response:any)=>{
                    })
            })
                
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