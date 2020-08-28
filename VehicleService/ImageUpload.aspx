<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ImageUpload.aspx.cs" Inherits="VehicleService.ImageUpload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/cropper/4.1.0/cropper.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/cropper/4.1.0/cropper.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cropper/1.0.1/jquery-cropper.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            var canvas = $("#imageCanvas");
            context = canvas.get(0).getContext("2d");

            $('#imageLoader').on('change', function () {
                if (this.files && this.files[0]) {
                    if (this.files[0].type.match(/^image\//)) {
                        var reader = new FileReader();
                        reader.onload = function (evt) {
                            var img = new Image();
                            img.onload = function () {
                                context.canvas.height = img.height;
                                context.canvas.width = img.width;
                                context.drawImage(img, 0, 0);

                                // Destroy the old cropper instance
                                canvas.cropper('destroy');

                                // Replace url
                                canvas.attr('src', this.result);

                                var cropper = canvas.cropper({
                                    //these options can be changed or modified according to need
                                    viewMode: 0,
                                    cropBoxResizable: false,
                                    minCropBoxWidth: 100,
                                    minCropBoxHeight: 100,
                                    dragMode: 'none',
                                    preview: $('#preview'),
                                    aspectRatio: 1,
                                    crop: function (e) {
                                        $('#crop_x').val(e.x);
                                        $('#crop_y').val(e.y);
                                    }
                                });
                            };
                            img.src = evt.target.result;
                        };
                        reader.readAsDataURL(this.files[0]);
                    } else {
                        alert("Invalid file type! Please select an image file.");
                    }
                } else {
                    alert('No file(s) selected.');
                }
            });
            /*
            $("#teh").cropper({
                preview: '#preview',
                dragMode: 'crop',
                autoCropArea: 0.65,
                rotatable: true,
                cropBoxMovable: true,
                cropBoxResizable: true
            });
            */
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <label>Image File:</label><br />
    <input type="file" id="imageLoader" name="imageLoader" />
    <canvas id="imageCanvas"></canvas>
    
    <div id="preview"></div>
    <div id="result"></div>


    <!--<img id="teh" src="_Dependencies/Images/CampbellAutoRepairs.png" />-->

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
