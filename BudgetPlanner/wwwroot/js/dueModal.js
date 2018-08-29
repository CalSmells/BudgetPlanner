connection.on("subDue", (names) => {
    var ul = document.getElementById("subDueModalBody");
    ul.innerHTML = "";
    for (var name of names) {
        var li = document.createElement("li");
        li.textContent = name;
        ul.append(li);
    }
    console.log(names);
    $("#subDueModal").modal();
})