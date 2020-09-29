namespace com.mercaderia.bono.Entidades.Utilidades
{
    public static class ExtensionesBasicas
    {
        /// <summary>
        /// extention that validates whether an object is null
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static bool IsNull(this object objeto)
        { return objeto == null || objeto is System.DBNull; }
    }
}
