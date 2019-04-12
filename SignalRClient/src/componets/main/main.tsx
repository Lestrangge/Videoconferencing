import * as React from 'react'
import Core from "src/core/core"
import Auth from "../auth/auth"
import { toast } from 'react-toastify';
import Invoker from "./../invoker/invoker"
interface IState {
    isLoggedIn: boolean;
}

export default class Main extends React.Component<any, IState>{
    constructor(props: any){
        super(props)
        this.state = {isLoggedIn: false};
        this.onAuthorized = this.onAuthorized.bind(this);
    }
    onAuthorized(){
        this.setState({isLoggedIn: true});
    }
    render(){
        return  <div>
                        {this.state.isLoggedIn ? 
                            <Invoker/>: 
                            <Auth ref="auth" onAuthorized={()=> this.onAuthorized()}/>
                        }
                </div>
    }
}