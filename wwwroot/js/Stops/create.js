function SubmitCreateForm() {
    nazev = $("#nazevInput")[0].value.trim()
    souradniceX = parseFloat($("#souradniceXInput")[0].value.trim())
    souradniceY = parseFloat($("#souradniceYInput")[0].value.trim())
    idPasmo = parseFloat($("#idPasmoInput")[0].value.trim())
    if (nazev.length > 0 && souradniceX && souradniceY && idPasmo)
        $.ajax({
            url: "CreateSubmit", // URL
            type: "Post",
            contentType: "application/json",
            data: JSON.stringify({ Nazev: nazev, SouradniceX: souradniceX, SouradniceY: souradniceY, IdPasmo: idPasmo }),
            statusCode: {
                200: (data) => {
                    console.log(data)
                    document.title = "BCSH2BDAS2 - " + data.titleName
                }
            }
        });
    return false
}