var nazevInput, souradniceXInput, souradniceYInput, idPasmoInput
const id = Number(sessionStorage.getItem("itemId"))

window.onload = () => {
    nazevInput = $("#nazevInput")[0]
    souradniceXInput = $("#souradniceXInput")[0]
    souradniceYInput = $("#souradniceYInput")[0]
    idPasmoInput = $("#idPasmoInput")[0]
    $.ajax({
        url: "EditModel",
        type: "Post",
        data: { id: id },
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
            url: "EditSubmit",
            type: "Post",
            contentType: "application/json",
            data: JSON.stringify({ IdZastavka: id, Nazev: nazev, SouradniceX: souradniceX, SouradniceY: souradniceY, IdPasmo: idPasmo }),
            statusCode: {
                200: () => window.location.href = "/Stops"
            }
        });
    return false
}