window.appAlerts = {
    success: function (title, text) {
        if (typeof Swal === "undefined") {
            alert((title || "Éxito") + "\n" + (text || ""));
            return;
        }

        return Swal.fire({
            icon: "success",
            title: title || "Éxito",
            text: text || "",
            confirmButtonText: "Aceptar"
        });
    },

    error: function (title, text) {
        if (typeof Swal === "undefined") {
            alert((title || "Error") + "\n" + (text || ""));
            return;
        }

        return Swal.fire({
            icon: "error",
            title: title || "Error",
            text: text || "",
            confirmButtonText: "Aceptar"
        });
    },

    warning: function (title, text) {
        if (typeof Swal === "undefined") {
            alert((title || "Atención") + "\n" + (text || ""));
            return;
        }

        return Swal.fire({
            icon: "warning",
            title: title || "Atención",
            text: text || "",
            confirmButtonText: "Aceptar"
        });
    },

    info: function (title, text) {
        if (typeof Swal === "undefined") {
            alert((title || "Información") + "\n" + (text || ""));
            return;
        }

        return Swal.fire({
            icon: "info",
            title: title || "Información",
            text: text || "",
            confirmButtonText: "Aceptar"
        });
    },

    confirm: async function (title, text, confirmButtonText) {
        if (typeof Swal === "undefined") {
            return confirm((title || "Confirmar") + "\n" + (text || ""));
        }

        const result = await Swal.fire({
            icon: "question",
            title: title || "Confirmar",
            text: text || "",
            showCancelButton: true,
            confirmButtonText: confirmButtonText || "Confirmar",
            cancelButtonText: "Cancelar"
        });

        return result.isConfirmed;
    }
};