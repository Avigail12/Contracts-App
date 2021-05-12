using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Contacts_App.Models;

namespace Contacts_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly contactsapp _context;

        public ContactsController(contactsapp context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contacts>>> GetContacts()
        {
            //var Contacts = await _context.Contacts.ToListAsync();
            return Ok(await _context.Contacts.ToListAsync());
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contacts>> GetContacts(int id)
        {
            var contacts = await _context.Contacts.FindAsync(id);

            if (contacts == null)
            {
                return NotFound();
            }

            return Ok(contacts);
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContacts(int id, Contacts contacts)
        {
            if (id != contacts.Id)
            {
                return BadRequest();
            }

            //contacts.UpdatedAt = DateTime.Now// TimeSpan.TryParse(DateTime.Now, out DateTime d);
            _context.Entry(contacts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Contacts
        [HttpPost]
        public async Task<ActionResult<Contacts>> PostContacts(Contacts contact)
        {
            //var Contacts = _context.Contacts.AsQueryable();

            //בד"כ יש אפשרות להגדיר ב מאפייני השדה בטבלה מספור אוטומטי  
            int Id_Last = _context.Contacts.Max(c => c.Id);
            contact.Id = Id_Last + 1;

            //בד"כ בדירות ולידציה כאלה עושים מצד הקליינט  
            if (contact.Phone.Length > 10 || contact.Phone.Length < 9)
                return BadRequest("מספר טלפון צריך להיות בין 9-10 תווים");
            if (contact.Phone.Contains("-"))
                contact.Phone = contact.Phone.Remove(contact.Phone.IndexOf("-"),1);
            if (!contact.Phone.All(char.IsDigit))
                return BadRequest("מספר טלפון צריך להיות מספרים בלבד");

            if (contact.Name.Length > 30)
                return BadRequest("אורך השם מוגבל ל 30 תווים");

            if (contact.Title.Length > 10)
                return BadRequest("אורך הכותרת מוגבל ל 10 תווים");

            //contact.CreatedAt = DateTime.Now;
            _context.Contacts.Add(contact);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ContactsExists(contact.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetContacts", new { id = contact.Id }, contact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Contacts>> DeleteContacts(int id)
        {
            var contacts = await _context.Contacts.FindAsync(id);
            if (contacts == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contacts);
            await _context.SaveChangesAsync();

            return contacts;
        }

        private bool ContactsExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
