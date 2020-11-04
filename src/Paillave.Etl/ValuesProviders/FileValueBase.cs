using System;
using System.IO;
using System.Text.Json;

namespace Paillave.Etl.ValuesProviders
{
    public abstract class FileValueBase<TMetadata> : IFileValue 
        where TMetadata : IFileValueMetadata
    {
        public FileValueBase(TMetadata metadata) => Metadata = metadata;
        private object _lock = new object();
        private bool _isDeleted = false;
        public abstract string Name { get; }
        public TMetadata Metadata { get; }
        public virtual string SourceType => this.Metadata.Type;
        public Type MetadataType => typeof(TMetadata);
        IFileValueMetadata IFileValue.Metadata => this.Metadata;
        public void Delete()
        {
            lock (_lock)
            {
                if (!_isDeleted)
                {
                    this.DeleteFile();
                    _isDeleted = true;
                }
            }
        }
        protected abstract void DeleteFile();
        public abstract Stream GetContent();
    }
    public abstract class FileValueMetadataBase : IFileValueMetadata
    {
        public virtual string Type => this.GetType().Name.Replace("FileValueMetadata", "", StringComparison.InvariantCultureIgnoreCase);
    }
    public class NoSourceFileValueMetadata : FileValueMetadataBase
    {
        public NoSourceFileValueMetadata(string type) => Type = type;
        public override string Type { get; }
    }
}