// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//Creating dom element
    function createElement(elemName, name, value) {
        let elem = document.createElement(elemName);
        elem.setAttribute('type', "hidden");
        elem.setAttribute('name', name);
        elem.setAttribute('value', value);
        return elem;
}

//$(function () {
//    var img = new Image();
//    img.onload = function () {
//        ctx.drawImage(img, 0, 100);
//    };
//    img.classList.add("lower-layer");
//    img.src = "http://images.christmastimeclipart.com/images/2/1271716593176_1788/img_1271716593176_17881.jpg";
//    console.log(img);
//    var canvas = document.getElementById("canvas");
//    var ctx = canvas.getContext("2d");
//    var canvasOffset = $("#canvas").offset();
//    var offsetX = canvasOffset.left;
//    var offsetY = canvasOffset.top;
//    var canvasWidth = canvas.width;
//    var canvasHeight = canvas.height;
//    var isDragging = false;

//    function handleMouseDown(e) {
//        canMouseX = parseInt(e.clientX - offsetX);
//        canMouseY = parseInt(e.clientY - offsetY);
//        // set the drag flag
//        isDragging = true;
//    }

//    function handleMouseUp(e) {
//        canMouseX = parseInt(e.clientX - offsetX);
//        canMouseY = parseInt(e.clientY - offsetY);
//        // clear the drag flag
//        isDragging = false;
//    }

//    function handleMouseOut(e) {
//        canMouseX = parseInt(e.clientX - offsetX);
//        canMouseY = parseInt(e.clientY - offsetY);
//        // user has left the canvas, so clear the drag flag
//        //isDragging=false;
//    }

//    function handleMouseMove(e) {
//        canMouseX = parseInt(e.clientX - offsetX);
//        canMouseY = parseInt(e.clientY - offsetY);
//        // if the drag flag is set, clear the canvas and draw the image
//        if (isDragging) {
//            ctx.clearRect(0, 0, canvasWidth, canvasHeight);
//            ctx.drawImage(img, canMouseX - 128 / 2, canMouseY - 120 / 2, 128, 120);
//        }
//    }

//    $("#canvas").mousedown(function (e) { handleMouseDown(e); });
//    $("#canvas").mousemove(function (e) { handleMouseMove(e); });
//    $("#canvas").mouseup(function (e) { handleMouseUp(e); });
//    $("#canvas").mouseout(function (e) { handleMouseOut(e); });

//}); // end $(function(){});



function initCanvas() {
    $('.canvas-container').each(function (index) {

        var canvasContainer = $(this)[0];
        var canvasObject = $("canvas", this)[0];
        var url = $(this).data('floorplan');
        var canvas = window._canvas = new fabric.Canvas(canvasObject);

        canvas.setHeight(200);
        canvas.setWidth(500);
        canvas.setBackgroundImage(url, canvas.renderAll.bind(canvas));

        var imageOffsetX, imageOffsetY;

        function handleDragStart(e) {
            [].forEach.call(images, function (img) {
                img.classList.remove('img_dragging');
            });
            this.classList.add('img_dragging');


            var imageOffset = $(this).offset();
            imageOffsetX = e.clientX - imageOffset.left;
            imageOffsetY = e.clientY - imageOffset.top;
        }

        function handleDragOver(e) {
            if (e.preventDefault) {
                e.preventDefault();
            }
            e.dataTransfer.dropEffect = 'copy';
            return false;
        }

        function handleDragEnter(e) {
            this.classList.add('over');
        }

        function handleDragLeave(e) {
            this.classList.remove('over');
        }

        function handleDrop(e) {
            e = e || window.event;
            if (e.preventDefault) {
                e.preventDefault();
            }
            if (e.stopPropagation) {
                e.stopPropagation();
            }
            var img = document.querySelector('.furniture img.img_dragging');
            console.log('event: ', e);

            var offset = $(canvasObject).offset();
            var y = e.clientY - (offset.top + imageOffsetY);
            var x = e.clientX - (offset.left + imageOffsetX);

            var newImage = new fabric.Image(img, {
                width: img.width,
                height: img.height,
                left: x,
                top: y
            });
            canvas.add(newImage);
            return false;
        }

        function handleDragEnd(e) {
            [].forEach.call(images, function (img) {
                img.classList.remove('img_dragging');
            });
        }

        var images = document.querySelectorAll('.furniture img');
        [].forEach.call(images, function (img) {
            img.addEventListener('dragstart', handleDragStart, false);
            img.addEventListener('dragend', handleDragEnd, false);
        });
        canvasContainer.addEventListener('dragenter', handleDragEnter, false);
        canvasContainer.addEventListener('dragover', handleDragOver, false);
        canvasContainer.addEventListener('dragleave', handleDragLeave, false);
        canvasContainer.addEventListener('drop', handleDrop, false);
    });
}
initCanvas();
