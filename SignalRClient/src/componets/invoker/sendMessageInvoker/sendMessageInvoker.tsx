import * as React from "react"

interface IProps{
    onSendMessage: (text: string, groupGuid: string) => void;
}
interface IState {
    groupGuid: string,
    text: string,
}
export default class SendMessageInvoker extends React.Component<IProps, IState>{
    constructor(props: IProps){
        super(props);
        this.state = {
            groupGuid: "eb0d42c7-5aaa-4751-b949-fc5c3f5769b2",
            text: "Hello!"
        }
    }
    render(){
        return <div style={{display: "flex", flexDirection: "column", alignItems:"start", borderBottomColor: "gray", borderBottomWidth:"2px", borderBottomStyle: "solid"}}>
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
            <button style={{marginTop: "8px"}} onClick={()=>{this.props.onSendMessage(this.state.text, this.state.groupGuid)}}>Send message</button>
        </div>
    }
}