
    // Зачем мне нужно IHasKey, если у меня уже есть IEntity<TKey>? Потому что IHasKey может использоваться для сущностей, которые не являются полноценными IEntity, но все же хотят иметь сильный типизированный ключ. Это может быть полезно в случаях, когда вам нужно работать с объектами, которые не требуют всех возможностей IEntity, но все же хотят иметь уникальный идентификатор.

namespace EntityNexus.DomainModel.Interfaces.Core
{
    /// <summary>
    /// Интерфейс для сильного типизированного (strongly-typed) ключа.
    /// Позволяет создавать типобезопасные идентификаторы вместо примитивов (int, Guid и т.д.).
    /// </summary>
    /// <typeparam name="T">Тип значения ключа (например, int, Guid, string).</typeparam>
    public interface IKey<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Значение ключа.
        /// </summary>
        T Value { get; }
    }

    /// <summary>
    /// Маркерный интерфейс для сущностей, использующих сильный типизированный ключ <see cref="IKey{T}"/>.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа, реализующий <see cref="IKey{T}"/>.</typeparam>
    /// <remarks>
    /// Полезен в случаях, когда нужно работать с объектами, которым требуется сильный ключ,
    /// но не обязательно полная реализация <see cref="IEntity{T}"/>.
    /// </remarks>
    public interface IHasKey<TKey> : IEntity<TKey>
        where TKey : IKey<TKey>, IEquatable<TKey>;
}