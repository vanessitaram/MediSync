document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("loginForm");
    const alerta = document.getElementById("alertaLogin");

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

        const correo = document.getElementById("Correo").value.trim();
        const telefono = document.getElementById("Telefono").value.trim();
        const contrasena = document.getElementById("Contrasena").value.trim();

        alerta.style.display = "none";

        if (!correo && !telefono) {
            mostrarError("Debes ingresar tu correo o teléfono.");
            return;
        }

        if (telefono && (telefono.length !== 10 || isNaN(telefono))) {
            mostrarError("El teléfono debe tener 10 dígitos numéricos.");
            return;
        }

        if (!contrasena || contrasena.length < 8) {
            mostrarError("La contraseña debe tener al menos 8 caracteres.");
            return;
        }

        alerta.style.display = "block";
        alerta.textContent = "Verificando credenciales...";
        alerta.className = "alerta alerta-info";

        setTimeout(() => {
            alerta.className = "alerta alerta-exito";
            alerta.textContent = "Inicio de sesión exitoso. Redirigiendo...";
            setTimeout(() => {
                window.location.href = "/Expediente/Crear";
            }, 2000);
        }, 1500);
    });

    function mostrarError(mensaje) {
        alerta.textContent = mensaje;
        alerta.className = "alerta alerta-error";
        alerta.style.display = "block";
    }
});
