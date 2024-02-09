using AutoMapper;
using CoreTestFramework.Core.Common;
using CoreTestFramework.Northwind.Business;
using CoreTestFramework.Northwind.Entities.DTO;
using CoreTestFramework.Northwind.Entities.DTO.Common;
using CoreTestFramework.Northwind.Entities.Model;
using CoreTestFramework.Northwind.WebMvcUI.Extension;
using CoreTestFramework.Northwind.WebMvcUI.ViewModels;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoreTestFramework.WebMvcUI.Controllers
{
    public class CategoryController: Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration; //AppSettingJson üzerinden path okumak için configuration sınıfı enjekte ediyoruz.
        private readonly string fileBasePath;
        private readonly string uploadYil;
        private readonly string uploadAy; 
        public CategoryController(ICategoryService categoryService, IMapper mapper, IConfiguration configuration)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _configuration = configuration;

            fileBasePath = _configuration["UploadPath"];
            uploadYil = DateTime.Now.ToString("yyyy");
            uploadAy = DateTime.Now.ToString("MM");
        }
        public async Task<ActionResult> Index() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PageData(IDataTablesRequest request)
        {
            var result = new Result {Success = false};
            DataTablesResponse response;
            IQueryable<CategoryDTO> categories;
            try
            {
                if(request == null) {
                    result.Message = "İstek elde edilemedi.";
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    response =DataTablesResponse.Create(request,0,0, new List<CategoryDTO>());
                    return RedirectToAction("Index","Home");
                }

                var get_list_category = await _categoryService.GetCategoryListAsync();
                if ( get_list_category.Success == false)
                {
                    result.Message = "Kategoriler getirilirken bir hata oluştu";
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    return RedirectToAction("Index","Home");
                }
                var mapped_list_category = _mapper.Map<List<CategoryDTO>>(get_list_category.Data);
                categories = mapped_list_category.AsQueryable();

                //Kullanıcı arama kutusuna bir değer girmişmi datatable kontrol ediliyor.
                if (!string.IsNullOrEmpty(request.Search.Value))
                {
                    var upperSearchValue = request.Search.Value.ToUpper(); //Büyük harf denetimi yapıyoruz.
                    var searchValue = request.Search.Value;
                    categories = categories.Where(c => c.category_name.ToUpper().Contains(upperSearchValue) || c.category_name.Contains(searchValue)).AsQueryable(); //Arama şartına göre ilgili entity de sorgulama yapıyoruz.
                }
                
                var dataPage = categories;
                
                var orderColumns = request.Columns.Where(c => c.IsSortable == true && c.Sort != null); //DataTable sıralama yapılan bir kolon varmı ona bakıyoruz.
                var searchColumns = request.Columns.Where(c => c.IsSearchable); //DataTable arama yapılan bir kolon var mı ona bakıyoruz.
                if (orderColumns.Count() == 0)
                {
                    dataPage.Take(request.Length); //Sıralama tabi özel bir kolon yoksa default ilerliyoruz.
                }
                else {
                    if (request.Length > 0)
                    {
                        dataPage.OrderBy(orderColumns).Skip(request.Start).Take(request.Length); //Sıralamaya tabi özel bir kolon varsa onu alıyoruz.
                    }
                    else
                        dataPage.OrderBy(orderColumns);
                }
                response = DataTablesResponse.Create(request, categories.Count(),categories.Count(),dataPage);
                return new DataTablesJsonResult(response);
            }
            catch (System.Exception ex)
            {
                result.Message = "Sistemsel bir hata oluştu.";
                TempData["result"] = JsonConvert.SerializeObject(result);
                return RedirectToAction("Index","Home");
            }
        }
        public async Task<ActionResult> Edit(int id) 
        {
            var result = new Result { Success = false };
            var viewModel = new CategoryViewModel();
            try
            {
                if (id == 0)
                {
                    result.Message = "Seçili bir kategori bulunamadı.";
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    return RedirectToAction("Index");
                }
                var get_category = await _categoryService.GetCategoryAsync(id);
                if (get_category.Success == false)
                {
                    result.Message ="Seçili kategori getirilirken bir hata oluştu.";
                    TempData["result"] = JsonConvert.SerializeObject(result);
                    return RedirectToAction("Index");
                }
                viewModel.Category = _mapper.Map<CategoryDTO>(get_category.Data);
                return PartialView("Edit",viewModel);
            }
            catch (System.Exception ex)
            {
                result.Message = "İstek elde edilemedi.";
                TempData["result"] = JsonConvert.SerializeObject(result);
                return View("Index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel categoryViewModel)
        {
            var result = new Result {Success = false};
            try
            {
               var category = await _categoryService.GetCategoryAsync(categoryViewModel.Category.category_id);
               category.Data.category_name = categoryViewModel.Category.category_name;
               category.Data.description = categoryViewModel.Category.description;
               if (categoryViewModel.uploadFiles != null)
               {
                    if (System.IO.File.Exists(Path.Combine(fileBasePath,uploadYil,uploadAy,categoryViewModel.uploadFiles[0].originalName)))
                    {
                        category.Data.picture = Path.Combine(fileBasePath,uploadYil,uploadAy,categoryViewModel.uploadFiles[0].originalName);
                    }
               }
                var add_category_result = await _categoryService.UpdateCategoryAsync(category.Data);
                if (add_category_result.Success)
                {
                    result.Message = "Kategori güncelleme işlemi başarıyla gerçekleşti";
                    result.Success = true;
                }
            }
            catch (System.Exception)
            {
                result.Message = "Kategori güncelleme işlemi başarısız oldu.";
            }

            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            var fileExist = Request.Form.Files.Count > 0; //Request içerisinde gelen dosya/dosyalar var mı ona bakıyoruz.
            try
            {
               
                if (fileExist)
                {
                    for (int i = 0; i < Request.Form.Files.Count; i++) //Request
                    {
                        var httpPostFile = Request.Form.Files[i];
                        if (httpPostFile != null)
                        {
                            
                            string uploadFileName = httpPostFile.FileName;
                            
                            if (!Directory.Exists(Path.Combine(fileBasePath, uploadYil))) Directory.CreateDirectory(Path.Combine(fileBasePath,uploadYil)); //O yıla ait klasor var mı yoksa oluşturuyoruz
                            if(!Directory.Exists(Path.Combine(fileBasePath,uploadYil,uploadAy))) Directory.CreateDirectory(Path.Combine(fileBasePath,uploadYil,uploadAy));//Yıl/Ay formatında ay ait klasor var mı yoksa oluşturuyoruz.
                            string filePath = Path.Combine(fileBasePath,uploadYil,uploadAy,uploadFileName); //Upload/Yıl/Ay/FileName formatında oluşturduğum klasor yolu

                            var fileInfo = new FileInfo(filePath);
                            var dosyaUzantisi = fileInfo.Extension; //Yüklenen dosyanın uzantısı FileInfo sınıfı ile aldık.
                            
                            if (dosyaUzantisi == ".jpg" || dosyaUzantisi == ".jpeg" || dosyaUzantisi ==".png") //Dosya uzantısı istediğimiz tipte mi?
                            {
                                using (var stream = System.IO.File.Create(filePath))
                                {
                                    
                                    await httpPostFile.CopyToAsync(stream); // Stream sınıfını kullanarak IFormFile interface dosyayı kaydediyoruz.
                                }
                            }
                            else
                                return Json(new {success = "false"});
                        }
                    }
                }
                return Json(new {success="true"});  
            }
            catch (System.Exception)
            {
                 return Json(new {success="false"});  
            }
            
        }
        [HttpPost]
        public IActionResult DeleteFile(string fileName)
        {
            try
            {
                string fileBasePath = _configuration["UploadPath"];
                string uploadYil = DateTime.Now.ToString("yyyy");
                string uploadAy = DateTime.Now.ToString("MM");
                string filePath = Path.Combine(fileBasePath,uploadYil,uploadAy,fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                return Json(new {success = "true"});
            }
            catch (System.Exception)
            {
                return Json(new {success = "false"});
            }
        }
    }

}