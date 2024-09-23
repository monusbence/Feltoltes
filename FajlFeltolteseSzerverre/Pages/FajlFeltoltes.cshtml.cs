using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace FajlFeltolteseSzerverre.Pages
{
    public class FajlFeltoltesModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        [BindProperty]
        //Begyûjtjük a feltöltendõ fájl adatait

        public IFormFile Feltoltes { get; set; }

        public FajlFeltoltesModel(IWebHostEnvironment env, FociDbContext context)
        {
            _env = env;
        }
        public void OnGet()
        {
        }


        public IActionResult OnPost()
        {
            if (Feltoltes == null || Feltoltes.Length == 0) 
            {
                ModelState.AddModelError("Feltoles","Válassz ki egy állományt!");
                return Page();
            }
            var FeltoltesiKonyvtar = Path.Combine(_env.ContentRootPath, "uploads");
            var FeltoltendoAllomanyPath = Path.Combine(FeltoltesiKonyvtar, Feltoltes.FileName);

            using (var stream = new FileStream(FeltoltendoAllomanyPath, FileMode.Create)) 
            {
                Feltoltes.CopyTo(stream);
            }

            StreamReader sr = new StreamReader(FeltoltendoAllomanyPath);
            sr.ReadLine();
            while (!sr.EndOfStream) 
            {
                var sor = sr.ReadLine();
                var elemek = sor.Split();
                Meccs ujMeccs = new Meccs();
                ujMeccs.Fordulo = int.Parse(elemek[0]);
                //..

                _context.Meccsek.Add(ujMeccs);
            }

            sr.Close();
            _context.SaveChanges();
            return Page();
        }
    }
}
