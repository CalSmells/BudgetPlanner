const connection = new signalR.HubConnectionBuilder()
    .withUrl("/budgetHub")
    .build();
connection.start().catch(err => console.error(err.toString()));