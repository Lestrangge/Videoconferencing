import * as React from "react"
import Core from "../../core/core"

interface IState {
    core: Core,
    groupGuid: string,
    text: string,
}
export default class Invoker extends React.Component<any, IState>{

    constructor(props: any){
        super(props);
        this.state = {
            core: Core.getInstance(), 
            groupGuid: "eb0d42c7-5aaa-4751-b949-fc5c3f5769b2",
            text: "Hello!"
        }
        this.onSendMessage = this.onSendMessage.bind(this);
    }

    onSendMessage(){
        this.state.core.SendMessage(this.state.text, this.state.groupGuid)
            .then(res=> console.warn("Sent: ", res))
            .catch(er => console.error("Error sending message: ", er))
    }

    render(){
        return <div style={{display: "flex", flexDirection: "column", alignItems:"start"}}>
            <span>Send message</span>
            <div style={{display: "flex", flexDirection: "row", marginTop: "8px"}}>
                <span>groupGuid: string</span>
                <input 
                    value={this.state.groupGuid}
                    type="text"
                    onChange={(ev: React.ChangeEvent<HTMLInputElement>)=>this.setState({groupGuid: (ev.target as HTMLInputElement).value})}/>
            </div>
            <div style={{display: "flex", flexDirection: "row", marginTop: "8px"}}>
                <span>text: string</span>
                <input 
                    value={this.state.text}
                    type="text"
                    onChange={(ev: React.ChangeEvent<HTMLInputElement>)=>this.setState({groupGuid: (ev.target as HTMLInputElement).value})}/>
            </div>
            <button style={{marginTop: "8px"}} onClick={()=>{this.onSendMessage()}}>Send message</button>
        </div>
    }
}