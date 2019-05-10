using System.Collections.Generic;
using System.IO;
using System.Text;
using AdminCore.Common.Interfaces;
using AutoMapper;
using ChoETL;

namespace AdminCore.DataETL
{
    public class ChoEtlAdapter : IDataEtlAdapter
    {
        private readonly IMapper _mapper;
        public ChoEtlAdapter(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        public string ToCsv<T, TMappedType>(IList<T> data) 
            where T : class
            where TMappedType : class
        {
            var mappedData = _mapper.Map<IList<TMappedType>>(data);
            
            StringBuilder msg = new StringBuilder();            
            using (var parser = new ChoCSVWriter<TMappedType>(new StringWriter(msg)))
            {
                parser.Write(mappedData);
            }

            return msg.ToString();
        }
    }
}