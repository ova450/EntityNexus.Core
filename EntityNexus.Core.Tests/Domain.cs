using EntityNexus.DomainModel.AbstractClasses.Core;
using EntityNexus.DomainModel.Interfaces.Core;

namespace EntityNexus.Samples.Core
{
    /// <summary>
    /// Пример доменных сущностей для демонстрации и тестирования возможностей EntityNexus.
    /// </summary>
    public static class Domain
    {
        /// <summary>
        /// Пример простой именованной сущности.
        /// </summary>
        public class Category : AEntityNamed
        {
            /// <summary>
            /// Создаёт новую категорию с указанным именем.
            /// </summary>
            /// <param name="name">Название категории.</param>
            public Category(string name) : base(name) { }
        }

        /// <summary>
        /// Пример сущности с иерархией (родитель-дети).
        /// </summary>
        public class Product : AEntity, IHasParent<Category>, IHasChild<Product>
        {
            /// <summary>
            /// Идентификатор родительской категории.
            /// </summary>
            public int ParentId { get; set; }

            /// <summary>
            /// Навигационное свойство на родительскую категорию.
            /// </summary>
            public Category? Parent { get; set; }

            /// <summary>
            /// Коллекция дочерних продуктов (например, варианты товара).
            /// </summary>
            public ICollection<Product> Children { get; set; } = new List<Product>();
        }
    }
}