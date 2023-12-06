using Product.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Handlers
{
    static public class Operations
    {
        static public List<Model.Product> DoSort(List<Model.Product> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.Name).ToList();
                else
                    items = items.OrderByDescending(n => n.Name).ToList();
            }
            else if (SortProperty.ToLower() == "price")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.Price).ToList();
                else
                    items = items.OrderByDescending(n => n.Price).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.Quantity).ToList();
                else
                    items = items.OrderByDescending(d => d.Quantity).ToList();
            }

            return items;
        }
    }
}
