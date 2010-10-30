function GenerateGUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    }).toUpperCase();
}

function LoadSpinnerImg(fogloaderimg) {
    $('#' + fogloaderimg).fogLoader({
        style: 'progressbar',
        width: 34,
        height: 37,
        progressValue: 100,
        progressBarImage: rootPath + 'Scripts/jquery.fogLoader.0.9.1/images/pbar.gif'
    });
}

function UnLoadSpinnerImg(fogloaderimg) {
    $('#' + fogloaderimg).fogLoader("destroy");
}

