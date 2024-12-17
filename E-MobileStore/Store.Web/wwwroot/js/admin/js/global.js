function callSuggestSearch(n) {
    if (!searching) {
        searching = true;
        n.preventDefault();
        var u = $(".click-search input").val().replace(inValidChar, ""),
            i = u.trim().toString().toLowerCase(),
            r = $(".list-sg-search"),
            t = $('#product-list');

        if (i.length < MIN_SSKEYWORD_LENGTH) {
            t.removeClass("active");
            r.html("");
            searching = false;
            return;
        }
        console.log(urlRoot,"/Home/ProductSearchResults")
        if (i.length >= MIN_SSKEYWORD_LENGTH) {
            $.ajax({
                url: urlRoot + "/quan-li-san-pham/tim-kiem",
                type: "POST",
                data: { search: i },
                cache: false,
                beforeSend: function () { },
                success: function (data) {
                    console.log(data)
                    if (data && data.length > 0) {
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
                                <td><input type="checkbox" class="productCheckbox" data-product-url="${product.categoryName}" /></td>
                                <td>${index}</td>
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
                            index++;
                        });
                        productList.html("");
                        productList.html(htmlContent);
                        productList.fadeIn();
                        //t.html(htmlContent); // Update the content of element t with the products
                        //t.addClass("active");
                        //r.addClass("active");
                        //t.fadeIn();
                    } else {
                        t.html(''); // Clear the content if no results
                        t.removeClass("active");
                        r.removeClass("active");
                        t.fadeOut();
                    }
                },
                error: function (ex) {
                    // Handle errors if necessary
                    console.error('Lỗi khi gửi yêu cầu tìm kiếm sản phẩm.',ex);
                }
            });
            searching = false;
        }
    }
}
!function (n)
{
    var t = function (n, t, i)
    {
        "use strict"; var e, r;
        if (function ()
        { var t, i = { lazyClass: "lazyload", loadedClass: "lazyloaded", loadingClass: "lazyloading", preloadClass: "lazypreload", errorClass: "lazyerror", autosizesClass: "lazyautosizes", fastLoadedClass: "ls-is-cached", iframeLoadMode: 0, srcAttr: "data-src", srcsetAttr: "data-srcset", sizesAttr: "data-sizes", minSize: 40, customMedia: {}, init: !0, expFactor: 1.5, hFac: .8, loadMode: 2, loadHidden: !0, ricTimeout: 0, throttleDelay: 125 }; r = n.lazySizesConfig || n.lazysizesConfig || {}; for (t in i) t in r || (r[t] = i[t]) }(), !t || !t.getElementsByClassName) return { init: function () { }, cfg: r, noSupport: !0 }; var s = t.documentElement, ot = n.HTMLPictureElement, h = "addEventListener", u = "getAttribute", o = n[h].bind(n), f = n.setTimeout, it = n.requestAnimationFrame || f, k = n.requestIdleCallback, rt = /^picture$/i, st = ["load", "error", "lazyincluded", "_lazyloaded"], p = {}, ht = Array.prototype.forEach, c = function (n, t) { return p[t] || (p[t] = new RegExp("(\\s|^)" + t + "(\\s|$)")), p[t].test(n[u]("class") || "") && p[t] }, l = function (n, t) { c(n, t) || n.setAttribute("class", (n[u]("class") || "").trim() + " " + t) }, d = function (n, t) { var i; (i = c(n, t)) && n.setAttribute("class", (n[u]("class") || "").replace(i, " ")) }, g = function (n, t, i) { var r = i ? h : "removeEventListener"; i && g(n, t); st.forEach(function (i) { n[r](i, t) }) }, a = function (n, i, r, u, f) { var o = t.createEvent("Event"); return r || (r = {}), r.instance = e, o.initEvent(i, !u, !f), o.detail = r, n.dispatchEvent(o), o }, nt = function (t, i) { var f; !ot && (f = n.picturefill || r.pf) ? (i && i.src && !t[u]("srcset") && t.setAttribute("srcset", i.src), f({ reevaluate: !0, elements: [t] })) : i && i.src && (t.src = i.src) }, v = function (n, t) { return (getComputedStyle(n, null) || {})[t] }, ut = function (n, t, i) { for (i = i || n.offsetWidth; i < r.minSize && t && !n._lazysizesWidth;)i = t.offsetWidth, t = t.parentNode; return i }, y = function () { var n, i, r = [], s = [], u = r, e = function () { var t = u; for (u = r.length ? s : r, n = !0, i = !1; t.length;)t.shift()(); n = !1 }, o = function (r, o) { n && !o ? r.apply(this, arguments) : (u.push(r), i || (i = !0, (t.hidden ? f : it)(e))) }; return o._lsFlush = e, o }(), w = function (n, t) { return t ? function () { y(n) } : function () { var t = this, i = arguments; y(function () { n.apply(t, i) }) } }, ct = function (n) { var u, e = 0, h = r.throttleDelay, t = r.ricTimeout, o = function () { u = !1; e = i.now(); n() }, s = k && t > 49 ? function () { k(o, { timeout: t }); t !== r.ricTimeout && (t = r.ricTimeout) } : w(function () { f(o) }, !0); return function (n) { var r; ((n = n === !0) && (t = 33), u) || (u = !0, r = h - (i.now() - e), r < 0 && (r = 0), n || r < 9 ? s() : f(s, r)) } }, ft = function (n) { var t, u, r = 99, e = function () { t = null; n() }, o = function () { var n = i.now() - u; n < r ? f(o, r - n) : (k || e)(e) }; return function () { u = i.now(); t || (t = f(o, r)) } }, et = function () { var pt, ut, kt, et, dt, gt, ni, ot, st, lt, at, wt, oi = /^img$/i, si = /^iframe$/i, hi = "onscroll" in n && !/(gle|ing)bot/.test(navigator.userAgent), ci = 0, vt = 0, b = 0, k = -1, ti = function (n) { b--; (!n || b < 0 || !n.target) && (b = 0) }, ii = function (n) { return wt == null && (wt = v(t.body, "visibility") == "hidden"), wt || !(v(n.parentNode, "visibility") == "hidden" && v(n, "visibility") == "hidden") }, li = function (n, i) { var u, r = n, f = ii(n); for (ot -= i, at += i, st -= i, lt += i; f && (r = r.offsetParent) && r != t.body && r != s;)f = (v(r, "opacity") || 1) > 0, f && v(r, "overflow") != "visible" && (u = r.getBoundingClientRect(), f = lt > u.left && st < u.right && at > u.top - 1 && ot < u.bottom + 1); return f }, ri = function () { var w, n, o, c, a, f, v, l, d, h, y, p, i = e.elements; if ((et = r.loadMode) && b < 8 && (w = i.length)) { for (n = 0, k++; n < w; n++)if (i[n] && !i[n]._lazyRace) { if (!hi || e.prematureUnveil && e.prematureUnveil(i[n])) { yt(i[n]); continue } if ((l = i[n][u]("data-expand")) && (f = l * 1) || (f = vt), h || (h = !r.expand || r.expand < 1 ? s.clientHeight > 500 && s.clientWidth > 500 ? 500 : 370 : r.expand, e._defEx = h, y = h * r.expFactor, p = r.hFac, wt = null, vt < y && b < 1 && k > 2 && et > 2 && !t.hidden ? (vt = y, k = 0) : vt = et > 1 && k > 1 && b < 6 ? h : ci), d !== f && (gt = innerWidth + f * p, ni = innerHeight + f, v = f * -1, d = f), o = i[n].getBoundingClientRect(), (at = o.bottom) >= v && (ot = o.top) <= ni && (lt = o.right) >= v * p && (st = o.left) <= gt && (at || lt || st || ot) && (r.loadHidden || ii(i[n])) && (ut && b < 3 && !l && (et < 3 || k < 4) || li(i[n], f))) { if (yt(i[n]), a = !0, b > 9) break } else !a && ut && !c && b < 4 && k < 4 && et > 2 && (pt[0] || r.preloadAfterLoad) && (pt[0] || !l && (at || lt || st || ot || i[n][u](r.sizesAttr) != "auto")) && (c = pt[0] || i[n]) } c && !a && yt(c) } }, p = ct(ri), ui = function (n) { var t = n.target; if (t._lazyCache) { delete t._lazyCache; return } ti(n); l(t, r.loadedClass); d(t, r.loadingClass); g(t, fi); a(t, "lazyloaded") }, ai = w(ui), fi = function (n) { ai({ target: n.target }) }, vi = function (n, t) { var i = n.getAttribute("data-load-mode") || r.iframeLoadMode; i == 0 ? n.contentWindow.location.replace(t) : i == 1 && (n.src = t) }, yi = function (n) { var t, i = n[u](r.srcsetAttr); (t = r.customMedia[n[u]("data-media") || n[u]("media")]) && n.setAttribute("media", t); i && n.setAttribute("srcset", i) }, pi = w(function (n, t, i, e, o) { var s, h, v, c, p, w; (p = a(n, "lazybeforeunveil", t)).defaultPrevented || (e && (i ? l(n, r.autosizesClass) : n.setAttribute("sizes", e)), h = n[u](r.srcsetAttr), s = n[u](r.srcAttr), o && (v = n.parentNode, c = v && rt.test(v.nodeName || "")), w = t.firesLoad || "src" in n && (h || s || c), p = { target: n }, l(n, r.loadingClass), w && (clearTimeout(kt), kt = f(ti, 2500), g(n, fi, !0)), c && ht.call(v.getElementsByTagName("source"), yi), h ? n.setAttribute("srcset", h) : s && !c && (si.test(n.nodeName) ? vi(n, s) : n.src = s), o && (h || c) && nt(n, { src: s })); n._lazyRace && delete n._lazyRace; d(n, r.lazyClass); y(function () { var t = n.complete && n.naturalWidth > 1; (!w || t) && (t && l(n, r.fastLoadedClass), ui(p), n._lazyCache = !0, f(function () { "_lazyCache" in n && delete n._lazyCache }, 9)); n.loading == "lazy" && b-- }, !0) }), yt = function (n) { if (!n._lazyRace) { var f, t = oi.test(n.nodeName), e = t && (n[u](r.sizesAttr) || n[u]("sizes")), i = e == "auto"; (i || !ut) && t && (n[u]("src") || n.srcset) && !n.complete && !c(n, r.errorClass) && c(n, r.lazyClass) || (f = a(n, "lazyunveilread").detail, i && tt.updateElem(n, !0, n.offsetWidth), n._lazyRace = !0, b++, pi(n, f, i, e, t)) } }, wi = ft(function () { r.loadMode = 3; p() }), ei = function () { r.loadMode == 3 && (r.loadMode = 2); wi() }, bt = function () { if (!ut) { if (i.now() - dt < 999) { f(bt, 999); return } ut = !0; r.loadMode = 3; p(); o("scroll", ei, !0) } }; return { _: function () { dt = i.now(); e.elements = t.getElementsByClassName(r.lazyClass); pt = t.getElementsByClassName(r.lazyClass + " " + r.preloadClass); o("scroll", p, !0); o("resize", p, !0); o("pageshow", function (n) { if (n.persisted) { var i = t.querySelectorAll("." + r.loadingClass); i.length && i.forEach && it(function () { i.forEach(function (n) { n.complete && yt(n) }) }) } }); n.MutationObserver ? new MutationObserver(p).observe(s, { childList: !0, subtree: !0, attributes: !0 }) : (s[h]("DOMNodeInserted", p, !0), s[h]("DOMAttrModified", p, !0), setInterval(p, 999)); o("hashchange", p, !0);["focus", "mouseover", "click", "load", "transitionend", "animationend"].forEach(function (n) { t[h](n, p, !0) }); /d$|^c/.test(t.readyState) ? bt() : (o("load", bt), t[h]("DOMContentLoaded", p), f(bt, 2e4)); e.elements.length ? (ri(), y._lsFlush()) : p() }, checkElems: p, unveil: yt, _aLSL: ei } }(), tt = function () { var n, f = w(function (n, t, i, r) { var f, u, e; if (n._lazysizesWidth = r, r += "px", n.setAttribute("sizes", r), rt.test(t.nodeName || "")) for (f = t.getElementsByTagName("source"), u = 0, e = f.length; u < e; u++)f[u].setAttribute("sizes", r); i.detail.dataAttr || nt(n, i.detail) }), i = function (n, t, i) { var r, u = n.parentNode; u && (i = ut(n, u, i), r = a(n, "lazybeforesizes", { width: i, dataAttr: !!t }), r.defaultPrevented || (i = r.detail.width, i && i !== n._lazysizesWidth && f(n, u, r, i))) }, e = function () { var t, r = n.length; if (r) for (t = 0; t < r; t++)i(n[t]) }, u = ft(e); return { _: function () { n = t.getElementsByClassName(r.autosizesClass); o("resize", u) }, checkElems: u, updateElem: i } }(), b = function () { !b.i && t.getElementsByClassName && (b.i = !0, tt._(), et._()) }; return f(function () { r.init && b() }), e = { cfg: r, autoSizer: tt, loader: et, init: b, uP: nt, aC: l, rC: d, hC: c, fire: a, gW: ut, rAF: y }
    }(n, n.document, Date); n.lazySizes = t; "object" == typeof module && module.exports && (module.exports = t)
}("undefined" != typeof window ? window : {}); window.isTopZone = !0; var timmer, MIN_SSKEYWORD_LENGTH = 2, urlRoot = window.location.origin, searching = !1, inValidChar = /:|;|!|@@|#|\$|%|\^|&|\*|' |"|>|<|,|\?|`|~|\+|=|_|-|\(|\)|{|}|\[|\]|\\|\|/gi, isMobile = !1, isBeta = window.location.hostname === "beta.topzone.vn", isStaging = window.location.hostname === "staging.topzone.vn", isProduction = window.location.hostname === "www.topzone.vn"; $(document).ready(function () { if ($(".bg-sg").click(function () { if ($(".form-search").fadeOut(function () { $(".form-search").removeClass("active"); $(".click-search").removeClass("active"); $(".sg-search").removeClass("active") }), $("section.detail").length > 0 && $("div.product__coupon").length > 0 && $("div.product__coupon").css("display") == "block") return !1; $(".bg-sg").fadeOut(); $("body").css("overflow", "unset"); $(".search-cart").fadeIn(function () { $(".logo-topzone").fadeIn(); $(".menu li").removeClass("hidden") }); $(".sg-search").fadeOut() }), $(".menu li").click(function () { $(".menu li").addClass("active"); $(".menu li").not(this).removeClass("active") }), $(".bg-popup").click(function () { $(".slide-popup").removeClass("active"); $("body").css("overflow", "unset") }), $(".dmca-badge").length > 0) { var n = $(".dmca-badge").attr("href") + "&refurl=" + location.href; $(".dmca-badge").attr("href", n) } $(".topzone-search").click(function () { $(".form-search").fadeIn(function () { $(".form-search").addClass("active"); $(".click-search").addClass("active"); $(".click-search input").focus() }); $(".bg-sg").fadeIn(); $("body").css("overflow", "hidden"); $(".search-cart").fadeOut(function () { $(".logo-topzone").fadeOut(); $(".menu li").addClass("hidden") }) }); $(".click-search input").focus(function () { $(".history-txt").length == 0 && $(".list-sg-search").length == 0 && $(".text-search").length == 0 && ViewSearchKeywordHistory() }); $(".click-search input").keyup(function (n) { var r; n.preventDefault(); var u = $(".list-sg-search"), i = $(".sg-search"), t = $(".click-search input").val(); if (t = t.replace(inValidChar, ""), r = t.trim().toLowerCase(), r.length < MIN_SSKEYWORD_LENGTH) { i.hide(); u.removeClass("active"); i.html(""); return } if (n.which == 40 || n.which == 38) { UpDownSuggest(n.which); return } n.type == "submit" || n.which == 13 ? goToSearchPage(t) : searching || (clearTimeout(timmer), timmer = setTimeout(function () { callSuggestSearch(n) }, 600)) }); $(".submit-search").click(function (n) { var t, i; if (n.preventDefault(), t = $(".click-search input").val(), t = t.replace(inValidChar, ""), i = t.trim().toLowerCase(), i.length < MIN_SSKEYWORD_LENGTH) { $(".search-result").html(""); return } goToSearchPage(t) }); $(".topzone-delSearch").click(function () { $(".form-search").fadeOut(function () { $(".form-search").removeClass("active"); $(".click-search").removeClass("active"); $(".sg-search").removeClass("active") }); $(".bg-sg").fadeOut(); $("body").css("overflow", "unset"); $(".search-cart").fadeIn(function () { $(".logo-topzone").fadeIn(); $(".menu li").removeClass("hidden") }); $(".sg-search").fadeOut(function () { $(".sg-search").html("") }); $(".click-search input").val("") }); $(window).scroll(function () { $(window).scrollTop() > 200 ? ($("header").addClass("sticky"), $(".theme-lunaNewYear").addClass("fixed-bg")) : ($("header").removeClass("sticky"), $(".theme-lunaNewYear").removeClass("fixed-bg")) }); GetQuanatyCart(); $(".boxchat-balloons").click(function () { $(".chat-window").toggleClass("active"); let n = $(".chat-widget-container"), t = $(".chat-window ul li").not(":eq(0)"); n.find(".zalo-chat-widget").length == 0 ? $.ajax({ url: "/Common/GetChatWidget", method: "POST", beforeSend: function () { t.addClass("prevent") }, success: function (t) { n.html(t) }, complete: function () { setTimeout(function () { t.removeClass("prevent") }, 500); $(".zalo-chat-widget").addClass("active") } }) : $(".zalo-chat-widget").toggleClass("active") }); $(".boxchat-closewindow").click(function () { $(".chat-window").removeClass("active"); $(".zalo-chat-widget").removeClass("active") }) });