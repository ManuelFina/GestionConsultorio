window.appAlerts = {
    success: function (title, text) {
        return Swal.fire({
            icon: 'success',
            title: title,
            text: text,
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#2563eb'
        });
    },

    error: function (title, text) {
        return Swal.fire({
            icon: 'error',
            title: title,
            text: text,
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#2563eb'
        });
    },

    warning: function (title, text) {
        return Swal.fire({
            icon: 'warning',
            title: title,
            text: text,
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#2563eb'
        });
    },

    info: function (title, text) {
        return Swal.fire({
            icon: 'info',
            title: title,
            text: text,
            confirmButtonText: 'Aceptar',
            confirmButtonColor: '#2563eb'
        });
    },

    confirm: async function (title, text, confirmButtonText) {
        const result = await Swal.fire({
            icon: 'warning',
            title: title,
            text: text,
            showCancelButton: true,
            confirmButtonText: confirmButtonText || 'Confirmar',
            cancelButtonText: 'Cancelar',
            confirmButtonColor: '#dc2626',
            cancelButtonColor: '#64748b',
            reverseButtons: true
        });

        return result.isConfirmed;
    }
};