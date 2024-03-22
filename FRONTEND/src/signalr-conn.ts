import * as signalR from "@microsoft/signalr";
const URL = process.env.HUB_ADDRESS ?? "https://scapiweboscket.azurewebsites.net/chat?token="; //or whatever your backend port is
class Connector {
    private connection: signalR.HubConnection;
    static instance: Connector;
    constructor(token, onMessageReceived) {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(URL+token,{
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
              })
            .withAutomaticReconnect()
            .build();
        this.connection.start().catch(err => document.write(err));

            this.connection.on("SendResponse", (data) => {
                onMessageReceived(data);
            });

    }
    public newMessage = (device: string,seconds: number) => {
        this.connection.send("Freq",  seconds, device).then(x => console.log("sent"))
    }
    public static getInstance(token, OnMessage): Connector {
        if (!Connector.instance)
            Connector.instance = new Connector(token, OnMessage);
        return Connector.instance;
    }
}
export default Connector.getInstance;