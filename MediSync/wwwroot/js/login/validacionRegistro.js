document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("registroForm");
    const modal = document.getElementById("successModal");
    const btnIrLogin = document.getElementById("btnIrLogin");

    document.querySelectorAll(".toggle-pass").forEach(btn => {
        btn.addEventListener("click", () => {
            const targetId = btn.getAttribute("data-target");
            const input = document.getElementById(targetId);
            input.type = input.type === "password" ? "text" : "password";
            btn.textContent = input.type === "text" ? "🙈" : "👁";
        });
    });

    form.addEventListener("submit", e => {
        e.preventDefault();

        const nombre = document.getElementById("Nombre").value.trim();
        const edad = parseInt(document.getElementById("Edad").value);
        const sexo = document.getElementById("Sexo").value.trim().toUpperCase();
        const telefono = document.getElementById("Telefono").value.trim();
        const pass = document.getElementById("Contrasena").value;
        const confirm = document.getElementById("ConfirmarContrasena").value;
        const contacto = document.getElementById("NombreContacto").value.trim();
        const telContacto = document.getElementById("TelefonoContacto").value.trim();
        const parentesco = document.getElementById("ParentescoContacto").value.trim();

        if (!nombre || !edad || !sexo || !telefono || !pass || !confirm || !contacto || !telContacto || !parentesco) {
            alert("Por favor, completa todos los campos obligatorios.");
            return;
        }

        if (sexo !== "M" && sexo !== "F") {
            alert("El campo Sexo debe ser 'M' o 'F'.");
            return;
        }

        if (telefono.length !== 10 || isNaN(telefono)) {
            alert("El teléfono debe tener exactamente 10 dígitos.");
            return;
        }

        if (pass.length < 8) {
            alert("La contraseña debe tener al menos 8 caracteres.");
            return;
        }

        if (pass !== confirm) {
            alert("Las contraseñas no coinciden.");
            return;
        }

        if (telContacto.length !== 10 || isNaN(telContacto)) {
            alert("El teléfono del contacto debe tener 10 dígitos.");
            return;
        }

        modal.style.display = "flex";

        setTimeout(() => {
            window.location.href = "/Pacientes/Login";
        }, 3500);
    });

    btnIrLogin.addEventListener("click", () => {
        window.location.href = "/Pacientes/Login";
    });
});
