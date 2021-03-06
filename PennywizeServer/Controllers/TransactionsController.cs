using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PennywizeServer.Models;

namespace PennywizeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [RegisteredUser]
    public class TransactionsController : PennywizeControllerBase
    {
        private readonly PennywizeContext context;
        public TransactionsController(PennywizeContext context) => this.context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions() =>
            await context.Transactions
                .Where(t => t.UserId == PennywizeUser.Id)
                .ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(string id)
        {
            var transaction = await context.Transactions.FindAsync(id);

            if (transaction == null) return NotFound();
            if (transaction.UserId != PennywizeUser.Id) return Forbid();

            return transaction;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutTransaction(string id, Transaction transaction)
        {
            if (id != transaction.Id) return BadRequest();

            var t = await context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(tr => tr.Id == id);

            if (t == null) return NotFound();
            if (t.UserId != PennywizeUser.Id) return Forbid();
            if (transaction.UserId != null && transaction.UserId != t.UserId) return BadRequest();

            transaction.UserId ??= t.UserId;
            context.Entry(transaction).State = EntityState.Modified;

            try { await context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            if (transaction.UserId != null && transaction.UserId != PennywizeUser.Id) return Forbid();

            transaction.UserId ??= PennywizeUser.Id;
            context.Transactions.Add(transaction);

            try { await context.SaveChangesAsync(); }
            catch (DbUpdateException)
            {
                if (TransactionExists(transaction.Id)) return Conflict();
                else throw;
            }

            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, new { Id = transaction.Id, UserId = transaction.UserId });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaction>> DeleteTransaction(string id)
        {
            var transaction = await context.Transactions.FindAsync(id);

            if (transaction == null) return NotFound();
            if (transaction.UserId != PennywizeUser.Id) return Forbid();

            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();

            return transaction;
        }

        private bool TransactionExists(string id) => context.Transactions.Any(e => e.Id == id);
    }
}