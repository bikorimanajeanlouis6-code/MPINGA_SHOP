function createPdfBlob(base64) {
    if (typeof base64 !== "string" || base64.length === 0) {
        throw new Error("The PDF data is empty.");
    }

    const binary = atob(base64);
    const bytes = new Uint8Array(binary.length);
    for (let index = 0; index < binary.length; index++) {
        bytes[index] = binary.charCodeAt(index);
    }

    if (bytes.length < 5
        || bytes[0] !== 0x25
        || bytes[1] !== 0x50
        || bytes[2] !== 0x44
        || bytes[3] !== 0x46
        || bytes[4] !== 0x2d) {
        throw new Error("The generated file is not a valid PDF.");
    }

    return new Blob([bytes], { type: "application/pdf" });
}

const pendingPrintWindows = [];

document.addEventListener("click", event => {
    const button = event.target instanceof Element
        ? event.target.closest("button[data-pdf-print]")
        : null;

    if (!button || button.disabled || button.getAttribute("aria-disabled") === "true") {
        return;
    }

    const printWindow = window.open("about:blank", "_blank");
    if (!printWindow) {
        pendingPrintWindows.push(null);
        return;
    }

    printWindow.document.title = "Preparing PDF for printing";
    printWindow.document.body.textContent = "Preparing PDF for printing...";
    pendingPrintWindows.push(printWindow);
}, true);

window.customerReportDownloads = {
    downloadPdf(base64, fileName) {
        const file = createPdfBlob(base64);
        const url = URL.createObjectURL(file);
        const link = document.createElement("a");
        link.href = url;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        link.remove();
        window.setTimeout(() => URL.revokeObjectURL(url), 1000);
    },

    printPdf(base64) {
        const url = URL.createObjectURL(createPdfBlob(base64));
        const printWindow = pendingPrintWindows.shift();

        return new Promise((resolve, reject) => {
            if (!printWindow || printWindow.closed) {
                URL.revokeObjectURL(url);
                reject(new Error("The print window was blocked. Allow pop-ups for this site and try again."));
                return;
            }

            let completed = false;
            const cleanup = () => {
                if (completed) return;
                completed = true;
                window.clearTimeout(expiryTimeout);
                URL.revokeObjectURL(url);
            };

            const expiryTimeout = window.setTimeout(cleanup, 10 * 60 * 1000);
            printWindow.addEventListener("afterprint", cleanup, { once: true });
            printWindow.addEventListener("load", () => {
                try {
                    if (printWindow.closed) {
                        cleanup();
                        reject(new Error("The print window was closed before printing."));
                        return;
                    }

                    printWindow.focus();
                    printWindow.print();
                    resolve();
                } catch (error) {
                    reject(error);
                }
            }, { once: true });

            printWindow.addEventListener("beforeunload", cleanup, { once: true });
            printWindow.location.replace(url);
        });
    }
};