

window.createIconDiv = (containerId, imageUrl, x, y, width, height) => {
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

        container.appendChild(iconDiv);
    }
};