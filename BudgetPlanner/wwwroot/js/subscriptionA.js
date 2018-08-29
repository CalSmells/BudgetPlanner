function updateSubs() {
    connection.invoke("UpdateSubs", userId).catch(err => console.error(err.toString()));
}

connection.on("UpdateSubs", (ids) => {
    for (var id of ids) {
        var item = document.getElementById(`${id}`);
        item.textContent = "OVERDUE";
    }
})