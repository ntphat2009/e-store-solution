function loadCategories(uri) {
	$.ajax({
		type: "GET",
		url: uri,
		headers: {
			'X-Requested-With': 'XMLHttpRequest'
		},
		success: function (data) {
			var categoryList = $('#category-list');
			console.log(data);
			var htmlContent = "";
			data.forEach(function (category) {
				var IsActive = category.isActive;
				var IsDelete = category.isDeleted;
				if (IsActive == true && IsDelete == false) {
					var status = "<button class='pd-setting'>Hoạt động</button>";
				}
				else if (IsActive == false && IsDelete == false) {
					status = "<button class='ps-etting'>Tạm dừng</button>";
				}
				else {
					status = "<button class='ds-setting'>Đã xóa</button>";
				}
				var totalProduct = category.products.count();
				htmlContent += `
													<tr>
														<td><input type="checkbox" class="productCheckbox" data-category-url="${category.categoryUrl}" /></td>
														 <td>
														 <img class="lazyload" data-src="${category.imageURL}" alt="${category.imageName}" /></td>
														  <td>${category.name} </td>
														 <td>
														  ${status}
														 </td>
														 <td>
																  ${category.position}
														 </td>
																 <td> ${totalProduct} </td>
																 <td>
																				 <a data-toggle="tooltip" type = "button" href = "/quan-li-nganh-hang/${category.categoryUrl}" title = "chi tiết" class="pd-setting-ed" > <i class="fa-solid fa-circle-info" > </i></a>
																				 <a data-toggle="tooltip" title = "cập nhật" class="pd-setting-ed" href="javascript:void(0);" onclick="openPopupEdit('${category.categoryUrl}')"> <i class="fa-solid fa-pen-to-square" aria-hidden="true"> </i></a >
																				 <a data-toggle="tooltip" href="javascript:void(0);" data-category-url="${category.categoryUrl}" id="deleted" title="Xóa" class="pd-setting-ed"><i class="fa-solid fa-trash" aria-hidden="true"></i></a>
																 </td>
													 </tr>`;
			});
			categoryList.html("");
			categoryList.html(htmlContent);
			categoryList.fadeIn();
		},
		error: function (xhr, status, error) {
			console.log(error);
			alert("check console to see proplem");
		}
	});
};
readImage(0);
function readImage(id) {
	document.getElementById(`insertImage_${id}`).addEventListener('change', function (event) {
		const file = event.target.files[0];
		const imgPreview = document.getElementById(`imgPreview_${id}`);
		if (file) {
			const reader = new FileReader();
			reader.onload = function (e) {
				imgPreview.src = e.target.result;
				imgPreview.style.display = 'block'; // Hiện ảnh đã chọn
			}
			reader.readAsDataURL(file);
		}
	});
}
function openPopupEdit(categoryUrl) {
	$.ajax({
		url: "/quan-li-nganh-hang/cap-nhat-nganh-hang",
		data: { categoryUrl: categoryUrl },
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
	var deleteButton = document.querySelectorAll("#deleted");
	var deleteSelected = document.getElementById("deleteSelected");
	var productCheckbox = document.querySelectorAll(".productCheckbox");
	if (deleteButton == null) { return }
	deleteButton.forEach(item => {
		item.addEventListener("click", function (e) {
			e.preventDefault();
			const categoryUrl = this.getAttribute("data-category-url");
			console.log(categoryUrl);
			if (confirm("Bạn muốn xóa sản phẩm này ?")) {
				if (confirm) {
					deleteCategory(categoryUrl);
				}

			}
		});
	});
	if (deleteSelected == null) { return }

	deleteSelected.addEventListener("click", function (e) {
		e.preventDefault();
		var str = [];
		var checkbox = document.querySelectorAll('#category-list tr td input[type="checkbox"]');
		var i = 0;
		if (checkbox == null) { return }
		checkbox.forEach(function (checkbox) {
			if (checkbox.checked) {
				var categoryUrl = checkbox.getAttribute("data-category-url");
				str[i] = categoryUrl;
				i++;
			}
		});
		console.log("str", str)
		if (str.length > 0) {
			conf = confirm('Bạn có muốn xóa các bản ghi này không ?');
			if (conf == true) {
				deleteCategories(str);
			}
		}
	});
	function deleteCategories(categoryUrls) {
		$.ajax({
			url: '/quan-li-nganh-hang/xoa-nhieu-nganh-hang',
			type: 'PUT',
			data: { categoryUrls: categoryUrls },
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
	function deleteCategory(categoryUrl) {
		console.log(categoryUrl)
		debugger
		$.ajax({
			url: '/quan-li-nganh-hang/xoa-nganh-hang',
			type: 'PUT',
			data: { categoryUrl: categoryUrl },
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
var imageUrl = document.getElementById("image-url");
var position = document.getElementById("position");
var description = document.getElementById("description");
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
	if (inputFile == null || inputFile.value === "") {
		if (imageUrl == null || imageUrl.value === "") {
			alert("vui lòng chọn ảnh!");
			event.preventDefault();
			return;
		}
	}
	if (name.value === "" || position.value === "" || description.value === "" || createdBy.value === "" || isActive.value === "") {
		alert("vui lòng nhập đầy đủ các trường!");
		event.preventDefault();
		return;
	}
}