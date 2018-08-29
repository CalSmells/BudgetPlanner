function ping() {
    connection.invoke("Ping");
}
connection.on("pong", () => {
    console.log("pong");
})