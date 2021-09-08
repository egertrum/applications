export default function gameView() {
    document.body.style.backgroundColor = "#FFFFF0";
    let content = document.createElement('div');
    content.id = "view-container";
    content.style.textAlign = "center";

    content.innerText = 'CONTAINER';

    return content;
}
