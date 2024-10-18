window.onload = function () {
    $.ajax({
        url: "CreateModel",
        type: "Post",
        data: { id: 5 },
        statusCode: {
            200: (data) => document.title = "BCSH2BDAS2 - " + data.titleName,
            400: (data) => window.alert(data),
            500: (data) => window.alert(data)
        }
    });
};

function SubmitCreateForm() {
    nazev = $("#nazevInput")[0].value.trim()
    souradniceX = parseFloat($("#souradniceXInput")[0].value.trim())
    souradniceY = parseFloat($("#souradniceYInput")[0].value.trim())
    idPasmo = parseFloat($("#idPasmoInput")[0].value.trim())
    if (nazev.length > 0 && souradniceX && souradniceY && idPasmo)
        $.ajax({
            url: "CreateSubmit",
            type: "Post",
            contentType: "application/json",
            data: JSON.stringify({ Nazev: nazev, SouradniceX: souradniceX, SouradniceY: souradniceY, IdPasmo: idPasmo }),
            statusCode: {
                200: () => window.location.href = "/Stops",
                400: (data) => window.alert(data),
                500: (data) => window.alert(data)
            }
        });
    return false
}