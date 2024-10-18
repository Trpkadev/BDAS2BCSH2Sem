window.onload = () => {
    $.ajax({
        url: "DetailsModel",
        type: "Post",
        data: { id: Number(sessionStorage.getItem("itemId")) },
        statusCode: {
            200: (data) => {
                const item = data.model
                $("#nazevInput")[0].value = item.nazev
                $("#souradniceXInput")[0].value = item.souradniceX
                $("#souradniceYInput")[0].value = item.souradniceY
                $("#idPasmoInput")[0].value = item.idPasmo

                document.title = "BCSH2BDAS2 - " + data.titleName
            },
            400: (data) => window.alert(data),
            500: (data) => window.alert(data)
        }
    });
}