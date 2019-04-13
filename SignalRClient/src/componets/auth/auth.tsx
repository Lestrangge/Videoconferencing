import * as React from 'react';
import Core from "../../core/core";
import { toast } from 'react-toastify';

interface IProps{
    onAuthorized: ()=>void;
}

interface IState{
    login: string;
    password: string;
    buttonDisabled: boolean;
}

export default class Auth extends React.Component<IProps, IState>{

    constructor(props: IProps){
        super(props);
        this.onSumbit = this.onSumbit.bind(this);
        this.state = {
            buttonDisabled: false,
            login: "pepetest",
            password: "faunus17"
        };
    }

    private onSumbit(){
        this.setState({buttonDisabled: true})
        Core.login(this.state.login, this.state.password)
            .then(()=>{
                toast.success("Success")
                this.props.onAuthorized();
                this.setState({buttonDisabled: false})
            })
            .catch(er => {
                toast.error
                this.setState({buttonDisabled: false})
            })
    }

    render(){
        return <div style={{display: "flex", flexDirection: "column", alignItems: "start"}}>
            Login form
            <input 
                type="text" 
                value={this.state.login}
                style={{marginTop: "24px"}}
                onChange={(ev: React.ChangeEvent<HTMLInputElement>)=>this.setState({login: (ev.target as HTMLInputElement).value})}/>
            <input type="text" 
                value={this.state.password}
                style={{marginTop: "8px"}}
                onChange={(ev: React.ChangeEvent<HTMLInputElement>)=>this.setState({password: (ev.target as HTMLInputElement).value})}/>
            <button onClick={()=> this.onSumbit()} 
                style={{marginTop: "8px"}}
                disabled={this.state.buttonDisabled}>
                Login
            </button>
        </div>
    }
}