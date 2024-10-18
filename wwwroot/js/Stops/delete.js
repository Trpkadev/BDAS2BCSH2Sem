window.onload = () => {
    $.ajax({
        url: "DeleteModel",
        type: "Post",
        data: { id: Number(sessionStorage.getItem("itemId")) },
        statusCode: {
            200: (data) => {
                const item = data.model
                $("#nazev")[0].innerHTML = item.nazev
                $("#souradniceX")[0].innerHTML = item.souradniceX
                $("#souradniceY")[0].innerHTML = item.souradniceY
                $("#idPasmo")[0].innerHTML = item.idPasmo

                document.title = "BCSH2BDAS2 - " + data.titleName
            },
            400: (data) => window.alert(data),
            500: (data) => window.alert(data)
        }
    });
}

function SubmitDeleteForm() {
    $.ajax({
        url: "DeleteSubmit",
        type: "Post",
        data: { id: Number(sessionStorage.getItem("itemId")) },
        statusCode: {
            200: () => window.location.href = "/Stops",
            400: (data) => window.alert(data),
            500: (data) => window.alert(data)
        }
    });
    return false
}