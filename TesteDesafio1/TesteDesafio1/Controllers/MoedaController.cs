using Microsoft.AspNetCore.Mvc;
using TesteDesafio1.Model;

namespace TesteDesafio1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoedaController : ControllerBase
    {

        //Cria uma lista estatica
        public static Stack<Moeda> _pilhaMoeda = new Stack<Moeda>();

        //Adiciona item na lista
        [HttpPost("AddItemFila")]
        public IActionResult AddItemFila([FromBody] List<Moeda> listaMoedas)
        {

            if (listaMoedas == null)
                return NotFound();

            listaMoedas.ForEach(Moeda => _pilhaMoeda.Push(Moeda));

            return Ok(listaMoedas);

        }

        //Recupera o ultimo item da fila e remove
        [HttpGet("GetItemFila")]
        public IActionResult GetItemFila()
        {
            if (_pilhaMoeda.Count == 0)
                return Ok(new { erro = "A lista de moedas esta vazia!" });

            var ultimaMoeda = _pilhaMoeda.Pop();
            return Ok(ultimaMoeda);
        }

    }
}
