const nazevInput = $("#nazevInput")[0]
const souradniceXInput = $("#souradniceXInput")[0]
const souradniceYInput = $("#souradniceYInput")[0]
const idPasmoInput = $("#idPasmoInput")[0]

window.onload = () => {
    $.ajax({
        url: "EditModel", // URL
        type: "Post",
        data: { id: Number(sessionStorage.getItem("itemId")) },
        statusCode: {
            200: (data) => {
                const item = data.model
                nazevInput.value = item.nazev
                souradniceXInput.value = item.souradniceX
                souradniceYInput.value = item.souradniceY
                idPasmoInput.value = item.idPasmo

                document.title = "BCSH2BDAS2 - " + data.titleName
            }
        }
    });
}

function SubmitEditForm() {
    nazev = nazevInput.value.trim()
    souradniceX = parseFloat(souradniceXInput.value.trim())
    souradniceY = parseFloat(souradniceYInput.value.trim())
    idPasmo = parseFloat(idPasmoInput.value.trim())
    //TODO error handling / error alerting
    if (nazev.length > 0 && souradniceX && souradniceY && idPasmo)
        $.ajax({
            url: "EditSubmit", // URL
            type: "Post",
            data: { Nazev: nazev, SouradniceX: souradniceX, SouradniceY: souradniceY, IdPasmo: idPasmo },
            statusCode: {
                200: (data) => {
                    document.title = "BCSH2BDAS2 - " + data.titleName
                }
            }
        });
    return false
}