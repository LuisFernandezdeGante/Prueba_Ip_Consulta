let map;

function mostrarMapa(lat, lon, country) {

    if (map) {
        map.remove();
    }

    map = L.map('map').setView([lat, lon], 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap'
    }).addTo(map);

    L.marker([lat, lon]).addTo(map)
        .bindPopup(`Ubicación: ${country}`)
        .openPopup();
}


async function consultIp() {
    const ip = document.getElementById("ipInput").value;

    if (!ip) {
        alert("Ingrese una IP válida");
        return;
    }

    try {
        const response = await fetch(`/api/ip/${ip}`);

        if (response.status === 409) {
            const modal = new bootstrap.Modal(
                document.getElementById('ipExistsModal')
            );
            modal.show();
            return;
        }

        if (!response.ok) {
            throw new Error("Error al consultar IP");
        }

        const data = await response.json();

        await loadHistory();

        mostrarMapa(data.latitude, data.longitude, data.country);

    } catch (error) {
        console.error("Error en consultIp:", error);
        alert("Ocurrió un error al consultar la IP");
    }
}



async function loadHistory() {
    try {
        const response = await fetch(`/api/ip/history`);

        if (!response.ok) {
            throw new Error("No se pudo cargar el historial");
        }

        const data = await response.json();

        const tbody = document.getElementById("tableBody");
        tbody.innerHTML = "";

        data.forEach(item => {
            tbody.innerHTML += `
                <tr>
                    <td>${item.id}</td>
                    <td>${item.company}</td>
                    <td>${item.ip}</td>
                    <td>${item.country}</td>
                    <td>${item.city}</td>
                    <td>${item.latitude}</td>
                    <td>${item.longitude}</td>
                    <td>${item.lenguaje}</td>
                    <td>${item.timeZone}</td>
                    <td>
                        <button class="btn btn-info btn-sm me-1"
                            onclick="verEnMapa(${item.latitude}, ${item.longitude}, '${item.country}')">
                            Ver mapa
                        </button>
                        <button class="btn btn-sm btn-danger"
                            onclick="deleteRecord(${item.id})">
                            Eliminar
                        </button>
                    </td>
                </tr>
            `;
        });

    } catch (error) {
        console.error("Error en loadHistory:", error);
        alert("No se pudo cargar el historial");
    }
}


async function deleteRecord(id) {
    if (!confirm("¿Eliminar registro?")) return;

    try {
        const response = await fetch(`/api/ip/${id}`, {
            method: "DELETE"
        });

        if (!response.ok) {
            throw new Error("Error al eliminar el registro");
        }

        await loadHistory();

    } catch (error) {
        console.error("Error en deleteRecord:", error);
        alert("No se pudo eliminar el registro");
    }
}

function filterTable() {
    const filter = document
        .getElementById("filterCountry")
        .value.toLowerCase();

    const rows = document.querySelectorAll("#ipTable tbody tr");

    rows.forEach(row => {
        const country = row.cells[3].textContent.toLowerCase();
        row.style.display = country.includes(filter) ? "" : "none";
    });
}

function verEnMapa(lat, lon, country) {
    mostrarMapa(lat, lon, country);
}

window.onload = loadHistory;