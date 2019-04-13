import * as React from "react"
import Core from "../../core/core"
import SendMessageInvoker from "./sendMessageInvoker/sendMessageInvoker"
import InitiateCallInvoker from "./initiateCallInvoker/initiateCallInvoker"
interface IState {
    core: Core
}
export default class Invoker extends React.Component<any, IState>{

    constructor(props: any){
        super(props);
        this.state = {
            core: Core.getInstance()
        }
        this.onSendMessage = this.onSendMessage.bind(this);
        this.onCallInvoked = this.onCallInvoked.bind(this);
    }

    onSendMessage(text: string, groupGuid: string){
        this.state.core.SendMessage(text, groupGuid)
            .then(res=> console.warn("Sent: ", res))
            .catch(er => console.error("Error sending message: ", er))
    }

    onCallInvoked(groupGuid: string, onLocalStream: (stream: MediaStream) => void, onRemoteStream: (stream: MediaStream)=> void){
        this.state.core.initCall(onLocalStream, onRemoteStream, groupGuid)
            .then(res=> console.warn("Call initiated!: ", res))
            .catch(er => console.error("Error sending message: ", er));
    }

    render(){
        return <div>
            <SendMessageInvoker onSendMessage={this.onSendMessage}/>
            <InitiateCallInvoker onCallInitiated={this.onCallInvoked}/>
        </div>
    }
}