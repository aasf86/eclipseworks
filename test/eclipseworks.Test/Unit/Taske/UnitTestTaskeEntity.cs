using static eclipseworks.Domain.Entities.Taske;
using Entity = eclipseworks.Domain.Entities;

namespace eclipseworks.Test.Unit.Taske
{
    public class UnitTestTaskeEntity
    {
        public UnitTestTaskeEntity() { }

        private void Setup() { }

        #region [Taske]

        [Fact(DisplayName = "[Taske] Não permitir alterar prioridade")]
        public void When_Try_Change_Priority_Then_Throw_Exception()
        {
            var taske = new Entity.Taske(
                "titulo", 
                "Descrição", 
                DateTime.Now.AddDays(365), 
                eStatus.Pending, 
                ePriority.Low, 
                1);

            ;

            Assert.Throws<InvalidDataException>(() => { taske.SetPriority(ePriority.High); });
        }

        #endregion

    }
}