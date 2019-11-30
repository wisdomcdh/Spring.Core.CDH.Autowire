using NUnit.Framework;
using Spring.Core.CDH.Autowire;

namespace Test
{
    public class SpringAutowireTest : TestWithSpring
    {
        [Test]
        public void Autowire_Autowire특성이선언된객체를생성하여_Autowire할때_해당속성은재귀적으로Autowire된다()
        {
            // Arrange
            var zoo = new Zoo();

            // Act
            SpringAutowire.Autowire(zoo);

            // Assert
            Assert.IsNotNull(zoo.Zookeeper);
            Assert.IsNotNull(zoo.Reptile);
            Assert.IsNotNull(zoo.Reptile?.Snake);
            Assert.IsNotNull(zoo.Reptile?.Turtle);
        }

        [Test]
        public void Autowire_Autowire특성이선언된객체의유형으로_Autowire할때_인스턴스의생성을하고속성들이재귀적으로Autowire된다()
        {
            // Act
            var zoo = (Zoo)SpringAutowire.Autowire(typeof(Zoo));

            // Assert
            Assert.IsNotNull(zoo.Zookeeper);
            Assert.IsNotNull(zoo.Reptile);
            Assert.IsNotNull(zoo.Reptile?.Snake);
            Assert.IsNotNull(zoo.Reptile?.Turtle);
        }

        [Test]
        public void Autowire_Autowire특성이선언된객체의유형으로_Autowire할때_인스턴스의생성을하고속성들이재귀적으로Autowire된다2()
        {
            // Act
            var zoo = SpringAutowire.Autowire<Zoo>();

            // Assert
            Assert.IsNotNull(zoo.Zookeeper);
            Assert.IsNotNull(zoo.Reptile);
            Assert.IsNotNull(zoo.Reptile?.Snake);
            Assert.IsNotNull(zoo.Reptile?.Turtle);
        }

        public class Zoo
        {
            [Autowire]
            public Zookeeper Zookeeper { get; set; }

            [Autowire]
            public Reptile Reptile { get; set; }
        }

        public class Reptile
        {
            [Autowire]
            public Snake Snake { get; set; }

            [Autowire]
            public Turtle Turtle { get; set; }
        }

        public class Zookeeper { }

        public class Snake { }

        public class Turtle { }
    }
}