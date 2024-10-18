window.onload = () => {
    $.ajax({
        url: "Stops/IndexModel", // URL
        type: "Post",
        statusCode: {
            200: (data) => {
                console.log(data)
                data.model.forEach((item) => {
                    const tr = document.createElement("tr")
                    const tdNazev = document.createElement("td")
                    const tdSouradniceX = document.createElement("td")
                    const tdSouradniceY = document.createElement("td")
                    const tdIdPasmo = document.createElement("td")
                    const tdButtons = document.createElement("td")
                    const buttonEdit = document.createElement("button")
                    const buttonDetails = document.createElement("button")
                    const buttonDelete = document.createElement("button")
                    tdNazev.innerHTML = item.nazev
                    tdSouradniceX.innerHTML = item.souradniceX
                    tdSouradniceY.innerHTML = item.souradniceY
                    tdIdPasmo.innerHTML = item.idPasmo
                    buttonEdit.innerHTML = "Edit"
                    buttonDetails.innerHTML = "Details"
                    buttonDelete.innerHTML = "Delete"

                    buttonEdit.onclick = () => {
                        sessionStorage.setItem("itemId", item.idZastavka)
                        window.location.href = "Stops/Edit"
                    }
                    buttonDetails.onclick = () => {
                        sessionStorage.setItem("itemId", item.idZastavka)
                        window.location.href = "Stops/Details"
                    }
                    buttonDelete.onclick = () => {
                        sessionStorage.setItem("itemId", item.idZastavka)
                        window.location.href = "Stops/Delete"
                    }

                    tdButtons.appendChild(buttonEdit)
                    tdButtons.appendChild(buttonDetails)
                    tdButtons.appendChild(buttonDelete)
                    tr.appendChild(tdNazev)
                    tr.appendChild(tdSouradniceX)
                    tr.appendChild(tdSouradniceY)
                    tr.appendChild(tdIdPasmo)
                    tr.appendChild(tdButtons)
                    $("#tbody")[0].appendChild(tr)
                })
                document.title = "BCSH2BDAS2 - " + data.titleName
            }
        }
    });
}