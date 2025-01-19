

window.createIconDiv = (containerId, imageUrl, x, y, width, height, color) => {
    const container = document.getElementById(containerId);

    if (container) {
        const iconDiv = document.createElement('div');

        iconDiv.style.position = 'absolute';
        iconDiv.style.left = `${x}px`;
        iconDiv.style.top = `${y}px`;
        iconDiv.style.width = `${width}px`;
        iconDiv.style.height = `${height}px`;
        iconDiv.style.backgroundImage = `url(${imageUrl})`;
        iconDiv.style.backgroundSize = 'cover';
        iconDiv.style.backgroundRepeat = 'no-repeat';
        iconDiv.style.border = "solid " + color;

        container.appendChild(iconDiv);
    }
};

window.clearIcons = (containerId) => {
    const container = document.getElementById(containerId);
    if (container) {
        // Remove all child elements (icons)
        container.innerHTML = '';
    }
};