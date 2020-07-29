using Newtonsoft.Json;
using SiteCurso.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;


namespace SiteCurso.Controllers
{
    public class UsuarioController : Controller
    {

        static IEnumerable<Cidade> cidade = null;

        private async System.Threading.Tasks.Task<IEnumerable<Cidade>> GetCidadesAsync()
        {
            IEnumerable<Cidade> cidade = null;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:62120/api/cidades");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                string token = await AutenticacaoUsuarios.getTokenAsync();
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                HttpResponseMessage resposta = await client.GetAsync(client.BaseAddress.ToString());

                if (resposta.IsSuccessStatusCode)
                {
                    var conteudo = resposta.Content.ReadAsStringAsync().Result;

                    cidade = JsonConvert.DeserializeObject<Cidade[]>(conteudo);

                }

            }

            return cidade;
        }




        // GET: Usuario
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {

            IEnumerable<Cidade> cidade = await GetCidadesAsync();

            IEnumerable<Usuario> usuarios = null;


            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:62120/api/usuarios");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                string token = await AutenticacaoUsuarios.getTokenAsync();
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                HttpResponseMessage resposta = await client.GetAsync(client.BaseAddress.ToString());

                if (resposta.IsSuccessStatusCode)
                {
                    var conteudo = resposta.Content.ReadAsStringAsync().Result;

                    usuarios = JsonConvert.DeserializeObject<Usuario[]>(conteudo);

                }
                return View(usuarios);
            }
        }



        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Usuario/Create
        public async System.Threading.Tasks.Task<ActionResult> Create()
        {
            cidade = await GetCidadesAsync();

            ViewBag.cod_cidade = new SelectList
            (
                cidade,
                "cod_cidade",
                "nome_cidade"
            );

            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(Usuario usuario)
        {
            try
            {

                if(!ModelState.IsValid)
                {
                    ViewBag.cod_cidade = new SelectList
                    (
                        cidade,
                        "cod_cidade",
                        "nome_cidade",
                        usuario.cod_cidade
                    );

                    return View(usuario);
                }


                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri("http://localhost:62120/api/usuarios");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                    string token = await AutenticacaoUsuarios.getTokenAsync();
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                    await client.PostAsJsonAsync(client.BaseAddress.ToString(), usuario);


                    return RedirectToAction("Index");
                }


            }
            catch
            {
                return View();
            }
            
            //return View();
        }

        // GET: Usuario/Edit/5
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id)
        {

            IEnumerable<Cidade> cidade = await GetCidadesAsync();

            Usuario usuario = null;


            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:62120/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                string token = await AutenticacaoUsuarios.getTokenAsync();
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                HttpResponseMessage resposta = await client.GetAsync("api/usuarios/"+id);

                if (resposta.IsSuccessStatusCode)
                {
                    var conteudo = resposta.Content.ReadAsStringAsync().Result;

                    usuario = JsonConvert.DeserializeObject<Usuario>(conteudo);

                }

                ViewBag.cod_cidade = new SelectList
                    (
                        cidade,
                        "cod_cidade",
                        "nome_cidade",
                        usuario.cod_cidade
                    );




                return View(usuario);
            }
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(int id, Usuario usuario)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ViewBag.cod_cidade = new SelectList
                    (
                        cidade,
                        "cod_cidade",
                        "nome_cidade",
                        usuario.cod_cidade
                    );

                    return View(usuario);
                }


                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri("http://localhost:62120/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                    string token = await AutenticacaoUsuarios.getTokenAsync();
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                    id = usuario.cod_cliente;
                    await client.PutAsJsonAsync("api/usuarios/" + id, usuario);


                    return RedirectToAction("Index");
                }


            }
            catch
            {

                return View();
            }
        }

        // GET: Usuario/Delete/5
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            
            Usuario usuario = null;


            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:62120/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                string token = await AutenticacaoUsuarios.getTokenAsync();
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                HttpResponseMessage resposta = await client.GetAsync("api/usuarios/" + id);

                if (resposta.IsSuccessStatusCode)
                {
                    var conteudo = resposta.Content.ReadAsStringAsync().Result;

                    usuario = JsonConvert.DeserializeObject<Usuario>(conteudo);

                }

             
                return View(usuario);
            }
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id,Usuario usuario)
        {
            try
            {



                if (id != usuario.cod_cliente)
                    return HttpNotFound();

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri("http://localhost:62120/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                    string token = await AutenticacaoUsuarios.getTokenAsync();
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                    

                    await client.DeleteAsync("api/usuarios/" + id);


                    return RedirectToAction("Index");
                }


            }
            catch
            {
                return View();
            }
        }
        
    }
}
