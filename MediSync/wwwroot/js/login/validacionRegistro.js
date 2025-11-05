document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("registroForm");

    // Mostrar / ocultar contraseñas
    document.querySelectorAll(".toggle-pass").forEach(btn => {
        btn.addEventListener("click", () => {
            const targetId = btn.getAttribute("data-target");
            const input = document.getElementById(targetId);
            if (input.type === "password") {
                input.type = "text";
                btn.textContent = "🙈";
            } else {
                input.type = "password";
                btn.textContent = "👁";
            }
        });
    });

    form.addEventListener("submit", e => {
        const pass = document.getElementById("Contrasena").value.trim();
        const confirm = document.getElementById("ConfirmarContrasena").value.trim();
        const telefono = document.getElementById("Telefono").value.trim();

        if (telefono && (!/^\d{10}$/.test(telefono))) {
            e.preventDefault();
            alert("El teléfono debe tener exactamente 10 dígitos.");
            return;
        }

        if (pass.length < 8) {
            e.preventDefault();
            alert("La contraseña debe tener al menos 8 caracteres.");
            return;
        }

        if (pass !== confirm) {
            e.preventDefault();
            alert("Las contraseñas no coinciden.");
            return;
        }
    });
});
