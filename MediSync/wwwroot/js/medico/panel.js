document.addEventListener("DOMContentLoaded", () => {
    const filas = document.querySelectorAll(".tabla-expedientes .fila");
    filas.forEach(fila => {
        fila.addEventListener("mouseenter", () => fila.classList.add("hover"));
        fila.addEventListener("mouseleave", () => fila.classList.remove("hover"));
    });
});
