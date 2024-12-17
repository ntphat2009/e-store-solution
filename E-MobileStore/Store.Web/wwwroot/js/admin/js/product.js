function loadProducts(uri) {
    $.ajax({
        type: "GET",
        url: uri,
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        success: function (data) {
            var productList = $('#product-list');
            console.log(data);
            var htmlContent = "";
            data.forEach(function (product, index) {
                var IsActive = product.isActive;
                var IsDelete = product.isDeleted;
                if (IsActive == true && IsDelete == false) {
                    var status = "<button class='pd-setting'>Hoạt động</button>";
                }
                else if (IsActive == false && IsDelete == false) {
                    status = "<button class='ps-etting'>Tạm dừng</button>";
                }
                else {
                    status = "<button class='ds-setting'>Đã xóa</button>";
                }
                var imagesList = product.productImages
                var imageActive = "";
                imagesList.forEach(function (image) {
                    if (image.position = "1") {
                        imageActive = image.imageURL;
                    }
                })
                var firstImageURL = product.productImages[0]?.imageURL || 'default-image.jpg';
                htmlContent += `
                            <tr>
                                <td><input type="checkbox" class="productCheckbox" data-product-url="${product.productUrl}" /></td>
                                <td><img class="lazyload" data-src="${imageActive}" alt="${imageActive}" /></td>
                                <td>${product.productName} </td>
                                 <td>
                                  ${status}
                                 </td>
                                 <td> ${product.categoryName} </td>
                                 <td> ${product.priceSale} đ </td>
                                 <td> ${product.price} đ </td>
                                 <td> ${product.quantity} </td>
                                 <td>
                                 <a data - toggle="tooltip" type = "button" href = "/quan-li-san-pham/${product.productUrl}" title = "chi tiết" class="pd-setting-ed" > <i class="fa-solid fa-circle-info" > </i></a>
                                         <a data - toggle="tooltip" title = "cập nhật" class="pd-setting-ed" href="javascript:void(0);" onclick="openPopupEdit('${product.productUrl}')"> <i class="fa-solid fa-pen-to-square" aria-hidden="true"> </i></a >
                                 <a data - toggle="tooltip" title = "Xóa" class="pd-setting-ed" > <i class="fa-solid fa-trash" aria-hidden="true"></i></a >
                                 </td>
                             </tr>`;
            });
            productList.html("");
            productList.html(htmlContent);
            productList.fadeIn();
        },
        error: function (xhr, status, error) {
            console.log(error);
            alert("check console to see proplem");
        }
    });
};
function openPopupEdit(productUrl) {
    $.ajax({
        url: "/quan-li-san-pham/cap-nhat-san-pham",
        data: { productUrl: productUrl },
        type: "GET",
        success: function (response) {
            $('#modalContent').html(response);
            $('#productModal').modal('show');
        },
        error: function () {
            alert("Có lỗi xảy ra khi tải dữ liệu.");
        }
    });
}
readImage(0);
var PictureWrap = document.getElementById("addPictureWrap");
function reassignFormIds() {
    const forms = document.querySelectorAll('[id^="form_"]');
    forms.forEach((form, index) => {
        // Re-index form
        form.id = `form_${index}`;
        // Assign id and name to inputs
        const inputs = form.querySelectorAll('input, select');
        inputs.forEach(input => {
            input.name = input.name.replace(/Images\[\d+\]/, `Images[${index}]`);
            if (input.id) {
                input.id = input.id.replace(/_\d+/, `_${index}`);
            }
        });

        // Assign id to remove button
        const removeButton = form.querySelector('[id^="remove_"]');
        if (removeButton) {
            removeButton.id = `remove_${index}`;
        }
    });
    const imgPreview = document.querySelectorAll('[id^="imgPreview_"]');
    imgPreview.forEach((img, index) => {
        // Re-index imgPreview_$
        img.id = `imgPreview_${index}`;
        console.log(`imgPreview_${index}`)

    });
};
document.addEventListener("click", function () {
    reassignFormIds();
}
);
//add form to update image
var insertMoreEdit = document.getElementById('insertMoreEdit');
if (insertMoreEdit) {
    insertMoreEdit.addEventListener('click', function () {
        let forms = document.querySelectorAll('[id^="form_"]');
        let formCount = forms.length - 1;
        const newForm = `
                        <div class="row imageform" id="form_${formCount}">
                            <div class="col-lg-3">
                                <div id="dropzone">
                                    <div class="input-group">
                                        <div class="input-group-append">
                                            <img id="imgPreview_${formCount}" class="lazyload" src="" alt="Preview Image" style="width:100%; height:auto; display:none;" />
                                            <input style="color:white" name="formFile" type="file" class="text-light" id="insertImage_${formCount}" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-9">
                                <div class="row">
                                    <div class="col-lg-5">
                                        <div class="input-group">
                                            <span class="input-group-addon">TT</span>
                                            <input type="text" name="Images[${formCount}].ImageName" class="form-control" placeholder="tên hình ảnh">
                                        </div>
                                        <div class="input-group" style="margin:5px 0px">
                                            <span class="input-group-addon">Vị trí sắp xếp</span>
                                            <select name="Images[${formCount}].Position" class="form-control pro-edt-select form-control-primary">
                                                <option value="1">Chọn vị trí sắp xếp</option>
                                                <option value="1">1</option>
                                                <option value="2">2</option>
                                                <option value="3">3</option>
                                                <option value="4">4</option>
                                                <option value="5">5</option>
                                                <option value="6">6</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="col-lg-3">
                                        <div class="input-group">
                                            <span class="input-group-addon">Trạng thái</span>
                                            <select name="Images[${formCount}].IsActive" class="form-control pro-edt-select form-control-primary">
                                                <option value="true">Hiển thị</option>
                                                <option value="false">Tạm Ẩn</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="col-lg-3">
                                        <div class="product-edt-remove" style="float:right;">
                                            <button type="button" id="remove_${formCount}" class="btn btn-ctl-bt waves-effect waves-light" style="background-color:red">
                                                Bỏ Ảnh
                                                <i class="fa fa-times" aria-hidden="true"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `;
        PictureWrap.insertAdjacentHTML('beforeend', newForm);
        const updatedForms = document.querySelectorAll('[id^="form_"]');
        console.log('Updated Forms:', updatedForms);
        readImage(formCount);
    });

}
//add form to add image
var insertMore = document.getElementById('insertMore');
if (insertMore) {
    insertMore.addEventListener('click', function () {
        debugger
        let forms = document.querySelectorAll('[id^="form_"]');
        let formCount = forms.length;
        const newForm = `
                            <div class="row imageform" id="form_${formCount}">
                                <div class="col-lg-3">
                                    <div id="dropzone">
                                        <div class="input-group">
                                            <div class="input-group-append">
                                                <img id="imgPreview_${formCount}" class="lazyload" src="" alt="Preview Image" style="width:100%; height:auto; display:none;" />
                                                <input style="color:white" name="formFile" type="file" class="text-light" id="insertImage_${formCount}" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-9">
                                    <div class="row">
                                        <div class="col-lg-5">
                                            <div class="input-group">
                                                <span class="input-group-addon">TT</span>
                                                <input type="text" name="Images[${formCount}].ImageName" class="form-control" placeholder="tên hình ảnh">
                                            </div>
                                            <div class="input-group" style="margin:5px 0px">
                                                <span class="input-group-addon">Vị trí sắp xếp</span>
                                                <select name="Images[${formCount}].Position" class="form-control pro-edt-select form-control-primary">
                                                    <option value="1">Chọn vị trí sắp xếp</option>
                                                    <option value="1">1</option>
                                                    <option value="2">2</option>
                                                    <option value="3">3</option>
                                                    <option value="4">4</option>
                                                    <option value="5">5</option>
                                                    <option value="6">6</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="col-lg-3">
                                            <div class="input-group">
                                                <span class="input-group-addon">Trạng thái</span>
                                                <select name="Images[${formCount}].IsActive" class="form-control pro-edt-select form-control-primary">
                                                    <option value="true">Hiển thị</option>
                                                    <option value="false">Tạm Ẩn</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="col-lg-3">
                                            <div class="product-edt-remove" style="float:right;">
                                                <button type="button" id="remove_${formCount}" class="btn btn-ctl-bt waves-effect waves-light" style="background-color:red">
                                                    Bỏ Ảnh
                                                    <i class="fa fa-times" aria-hidden="true"></i>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        `;
        PictureWrap.insertAdjacentHTML('beforeend', newForm);
        readImage(formCount);
    });
}
function readImage(id) {
    var insertImage = document.getElementById(`insertImage_${id}`)
    if (insertImage == null) { return; }
    insertImage.addEventListener('change', function (event) {
        const file = event.target.files[0];
        const imgPreview = document.getElementById(`imgPreview_${id}`);

        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                if (imgPreview.src) {
                    imgPreview.src = e.target.result;
                    imgPreview.style.display = 'block'; // Hiện ảnh đã chọn
                }
            }
            reader.readAsDataURL(file);
        }
    });
};
//remove form
if (PictureWrap) {
    PictureWrap.addEventListener('click', function (event) {
        if (event.target.matches('[id^="remove_"]')) {
            const formId = event.target.id.split('_')[1];
            var targetForm = document.getElementById(`form_${formId}`);
            if (targetForm != null) {
                targetForm.remove();
            }
        }
    });
} else {
    console.error("formnull");
}
document.addEventListener("DOMContentLoaded", function () {
    var deleteButton = document.querySelectorAll("#deleted");
    var deleteSelected = document.getElementById("deleteSelected");
    var productCheckbox = document.querySelectorAll(".productCheckbox");
    //deleted focus product
    if (deleteButton == null) { return }

    deleteButton.forEach(item => {
        item.addEventListener("click", function (e) {
            e.preventDefault();
            const productUrl = this.getAttribute("data-product-url");
            if (confirm("Bạn muốn xóa sản phẩm này ?")) {
                deleteProduct(productUrl);
            }
        });
    });
    //delete list
    if (deleteSelected == null) { return }
    deleteSelected.addEventListener("click", function (e) {
        e.preventDefault();
        var str = [];
        var checkbox = document.querySelectorAll('#product-list tr td input[type="checkbox"]');
        var i = 0;
        if (checkbox == null) { return }
        checkbox.forEach(function (checkbox) {
            if (checkbox.checked) {
                var productUrl = checkbox.getAttribute("data-product-url");
                str[i] = productUrl;
                i++;
            }
        });
        console.log("str", str)
        if (str.length > 0) {
            conf = confirm('Bạn có muốn xóa các bản ghi này không ?');
            if (conf == true) {
                $.ajax({
                    url: '/quan-li-san-pham/xoa-nhieu-san-pham',
                    type: 'PUT',
                    data: { productUrls: str },
                    success: function (rs) {
                        if (rs.success) {
                            location.reload();
                        }
                        else {
                            alert(`Đã có lỗi xảy ra khi xóa!  ${rs.error}`)
                        }
                    }
                });
            }
        }
    });

    function deleteProduct(productUrl) {
        console.log(productUrl)
        $.ajax({
            url: '/quan-li-san-pham/xoa-san-pham',
            type: 'PUT',
            data: { productUrl: productUrl },
            success: function (rs) {
                if (rs.success) {
                    location.reload();
                }
                else {
                    alert(`Đã có lỗi xảy ra khi xóa!  ${rs.error}`)
                }
            }
        });
    }
});
var name = document.getElementById("name");
var quantity = document.getElementById("quantity");
var price = document.getElementById("price");
var description = document.getElementById("description");
var categoryId = document.getElementById("category-id");
var shortDesc = document.getElementById("short-desc");
var createdBy = document.getElementById("created-by");
var isActive = document.getElementById("is-active");
var imageUrl = document.getElementById("image-url-0");
var imageName = document.getElementById("image-name-0");
var imagePosition = document.getElementById("position-0");
var isImageActive = document.getElementById("is-active-0");
var inputFile = document.getElementById(`insertImage_0`);
if (inputFile != null) {
    inputFile.addEventListener("change", function () {
        const filePath = this.value;
        const allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;
        if (!this.files.length) {
            alert("Vui lòng chọn ảnh");
            return;
        }
        if (!allowedExtensions.exec(filePath)) {
            alert('Chỉ chấp nhận các tệp .jpeg/.jpg/.png.');
            this.value = '';
            return;
        }
    });
}
function checkValidate(event) {
    if (inputFile == null || inputFile.value === "") {
        if (imageUrl == null || imageUrl.value === "") {
            alert("vui lòng chọn ảnh!");
            event.preventDefault();
            return;
        }
    }
    if (name.value === "" || imagePosition.value === "" || imageName.value === "" || isImageActive.value === "" || description.value === "" || createdBy.value === "" || isActive.value === "" || quantity.value === "" || price.value === "" || shortDesc.value === "" || categoryId.value === "") {
        alert("vui lòng nhập đầy đủ các trường!");
        event.preventDefault();
        return;
    }
}