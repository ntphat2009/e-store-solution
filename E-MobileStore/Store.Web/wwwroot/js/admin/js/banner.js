function loadBanner(uri) {
    $.ajax({
        type: "GET",
        url: uri,
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        success: function (data) {
            var bannerList = $('#banner-list');
            console.log(data);
            var htmlContent = "";
            data.forEach(function (banner, index) {
                var IsActive = banner.isActive;
                var IsDelete = banner.isDeleted;
                if (IsActive == true && IsDelete == false) {
                    var status = "<button class='pd-setting'>Hoạt động</button>";
                }
                else if (IsActive == false && IsDelete == false) {
                    status = "<button class='ps-etting'>Tạm dừng</button>";
                }
                else {
                    status = "<button class='ds-setting'>Đã xóa</button>";
                }
                var totalProduct = banner.products.count();
                htmlContent += `
                                    <tr>
                                        <td>${index}</td>
                                         <td>
                                         <img class="lazyload" data-src="${banner.imageURL}" alt="${banner.imageName}" /></td>
                                          <td>${banner.name} </td>
                                         <td>
                                          ${status}
                                         </td>
                                         <td>
                                                  ${banner.position}
                                         </td>
                                                 <td> ${totalProduct} </td>
                                         <td>
                                                 <a data-toggle="tooltip" type = "button" href = "/quan-li-banner/${banner.bannerUrl}" title = "chi tiết" class="pd-setting-ed" > <i class="fa-solid fa-circle-info" > </i></a>
                                                         <a data-toggle="tooltip" title = "cập nhật" class="pd-setting-ed" href="javascript:void(0);" onclick="openPopupEdit('${banner.bannerUrl}')"> <i class="fa-solid fa-pen-to-square" aria-hidden="true"> </i></a >
                                         <a data - toggle="tooltip" title = "Xóa" class="pd-setting-ed" > <i class="fa-solid fa-trash" aria-hidden="true"></i></a >
                                         </td>
                                     </tr>`;
                index++;
            });
            bannerList.html("");
            bannerList.html(htmlContent);
            bannerList.fadeIn();
        },
        error: function (xhr, status, error) {
            console.log(error);
            alert("check console to see proplem");
        }
    });
};

function openPopupEdit(bannerId) {
    $.ajax({
        url: "/quan-li-banner/cap-nhat-banner",
        data: { bannerId: bannerId },
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

document.addEventListener("DOMContentLoaded", function () {
    var getProductUrl = document.querySelectorAll("#deleted");
    getProductUrl.forEach(item => {
        item.addEventListener("click", function (e) {
            e.preventDefault();
            const bannerId = this.getAttribute("data-product-url");
            if (confirm("Bạn muốn xóa sản phẩm này ?")) {
                fetch(`/quan-li-banner/xoa-banner?bannerId=${bannerId}`, {
                    method: "PUT",
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            location.reload();
                        }
                        else {
                            alert("Đã có lỗi xảy ra khi xóa!")
                        }
                    })
                    .catch(error => {
                        console.error(error);
                        alert("Xóa sản phẩm thất bại.");
                    });
            }
        });
    });
});
readImage(0);
const imgPreview = document.getElementById(`imgPreview_${0}`);
if (imgPreview && imgPreview.src && imgPreview.src.trim() !== "") {
    imgPreview.style.display = 'block'; // Hiện ảnh đã chọn
}
function readImage(id) {
    const inputElement = document.getElementById(`insertImage_${id}`);
    const imgPreview = document.getElementById(`imgPreview_${id}`);

    if (inputElement && imgPreview) { // Kiểm tra xem phần tử có tồn tại hay không
        inputElement.addEventListener('change', function (event) {
            const file = event.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    imgPreview.src = e.target.result;
                    imgPreview.style.display = 'block'; // Hiển thị ảnh đã chọn
                }
                reader.readAsDataURL(file);
            }
        });
    } else {
        console.error(`Element insertImage_${id} or imgPreview_${id} not found.`);
    }
}
var bannerAlt = document.getElementById("banner-alt");
var imageUrl = document.getElementById("image-url");
console.log(bannerAlt.value)
var position = document.getElementById("position");
var categoryId = document.getElementById("category-id");
var createdBy = document.getElementById("created-by");
var isActive = document.getElementById("is-active");
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
    if (inputFile==null || inputFile.value === "") {
        if (imageUrl == null || imageUrl.value === "") {
            alert("vui lòng chọn ảnh!");
            event.preventDefault();
            return;
        }
    }
    if (bannerAlt.value === "" || position.value === "" || categoryId.value === "" || createdBy.value === "" || isActive.value === "") {
        alert("vui lòng nhập đầy đủ các trường!");
        event.preventDefault();
        return;
    }
}