using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Contracts.Domain.Base;

namespace Domain.Base
{
    public class LangString : LangString<Guid, Translation>, IDomainEntityId
    {
        public LangString()
        {
            
        }

        public LangString(string value, string? culture = null) : base(value, culture)
        {
            
        }
        public static implicit operator string(LangString? l) => l?.ToString() ?? "null";

        // LangString s = "Foo"
        public static implicit operator LangString(string s) => new LangString(s);
        
    }
    public class LangString<TKey, TTranslation> : DomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TTranslation: Translation<TKey>, new()
    {
        private static string _defaultCulture = "en";

        public virtual ICollection<TTranslation>? Translations { get; set; }
        
        public LangString()
        {
            
        }

        public LangString(string value, string? culture = null)
        {
            SetTranslation(value, culture);
        }

        public virtual void SetTranslation(string value, string? culture = null)
        {
            //Translations ??= new List<Translation>();
            culture ??= Thread.CurrentThread.CurrentUICulture.Name;
            
            if (Translations == null)
            {
                if (Id.Equals(default(TKey)))
                {
                    Translations = new List<TTranslation>();
                }
                else
                {
                    throw new NullReferenceException("Translations cannot be null. Did you forgot to do .include?");
                }
            }

            var translation = Translations.FirstOrDefault(t => t.Culture == culture);
            if (translation == null)
            {
                Translations.Add(new TTranslation()
                {
                    Value = value,
                    Culture = culture,
                });
            }
            else
            {
                translation.Value = value;
            }
        }

        public string? Translate(string? culture = null)
        {
            if (Translations == null)
            {
                if (Id.Equals(default(TKey)))
                {
                    return null;
                }
                throw new NullReferenceException("Translations cannot be null. Did you forgot to do .include?");
            }

            culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;
            
            /*
             cultures in db
             en, en-GB
             in query
             ru, en, en-US, en-GB
             */

            // do we have exact match
            var translation = Translations.FirstOrDefault(t => t.Culture == culture);
            if (translation != null)
            {
                return translation.Value;
            }
            
            // do we have match without region - match en-XX or en
            translation = Translations.FirstOrDefault(t => culture.StartsWith(t.Culture));
            if (translation != null)
            {
                return translation.Value;
            }
            
            // do we have the default culture string
            // exact match
            translation = Translations.FirstOrDefault(t => t.Culture == _defaultCulture);
            if (translation != null)
            {
                return translation.Value;
            }
            // starts with
            translation = Translations.FirstOrDefault(t => _defaultCulture.StartsWith(t.Culture));
            if (translation != null)
            {
                return translation.Value;
            }
            
            // just return the first one or null
            return Translations.FirstOrDefault()?.Value;
        }

        public override string ToString()
        {
            return Translate() ?? "??????";
        }
        
        // var x = "Foo" + new LangString(Bar)
        // x will be string FooBar
        public static implicit operator string(LangString<TKey, TTranslation>? l) => l?.ToString() ?? "null";

        // LangString s = "Foo"
        public static implicit operator LangString<TKey, TTranslation>(string s) => new LangString<TKey, TTranslation>(s);

    }

}
