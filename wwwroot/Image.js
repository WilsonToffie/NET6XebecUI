function getImage() {
    const imageUrl = "https://nebula123.sharepoint.com/sites/Project/Lists/Aneeqah%20List/Attachments/1/a.pdf";

    (async () => {
        const response = await fetch(imageUrl)
        const imageBlob = await response.blob()
        const reader = new FileReader();
        reader.readAsDataURL(imageBlob);
        reader.onloadend = () => {
            const base64data = reader.result;
            console.log(base64data);
        }
    })()
}