using System.Diagnostics;
using System.Numerics;
using System.Text;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.SPIRV;
using Veldrid.StartupUtilities;

namespace Hologram
{
    internal class Program
    {
        private static GraphicsDevice _graphicsDevice;

        private static CommandList _commandList;
        private static DeviceBuffer _vertexBuffer;
        private static DeviceBuffer _indexBuffer;
        private static Shader[] _shaders;
        private static Pipeline _pipeline;
        private static FrameManager _frameManager;

        static void Main(string[] args)
        {
            WindowCreateInfo windowCI = new WindowCreateInfo()
            {
                X = 100,
                Y = 100,
                WindowWidth = 1280,
                WindowHeight = 720,
                WindowTitle = "Hologram"
            };
            Sdl2Window window = VeldridStartup.CreateWindow(ref windowCI);

            GraphicsDeviceOptions options = new GraphicsDeviceOptions()
            {
                PreferStandardClipSpaceYDirection = true,
                PreferDepthRangeZeroToOne = true
            };

            _graphicsDevice = VeldridStartup.CreateGraphicsDevice(window, options);
            CreateResources();

            _frameManager = new FrameManager();

            while (window.Exists)
            {
                _frameManager.StartingDraw();
                window.PumpEvents();
                Draw();
                _frameManager.FinishedDraw();
                
                window.Title = "Hologram - " + _frameManager.avgFPS + " FPS";

                _frameManager.WaitUntilNextFrame();
            }
        }

        static void Draw()
        {
            _commandList.Begin();

            _commandList.SetFramebuffer(_graphicsDevice.SwapchainFramebuffer);

            _commandList.ClearColorTarget(0, RgbaFloat.Black);

            _commandList.SetVertexBuffer(0, _vertexBuffer);
            _commandList.SetIndexBuffer(_indexBuffer, IndexFormat.UInt16);
            _commandList.SetPipeline(_pipeline);
            _commandList.DrawIndexed(
                indexCount: _indexBuffer.SizeInBytes / 2,
                instanceCount: 1,
                indexStart: 0,
                vertexOffset: 0,
                instanceStart: 0);

            _commandList.End();
            _graphicsDevice.SubmitCommands(_commandList);
            _graphicsDevice.SwapBuffers();
        }

        static void DisposeResources()
        {
            _pipeline.Dispose();
            _commandList.Dispose();
            _vertexBuffer.Dispose();
            _indexBuffer.Dispose();
            _graphicsDevice.Dispose();
        }

        static void CreateResources()
        {
            ResourceFactory factory = _graphicsDevice.ResourceFactory;

            VertexPositionColor[] vertices = new VertexPositionColor[]
            {
                // Top
                new VertexPositionColor(new Vector3(-0.5f, +0.5f, -0.5f), RgbaFloat.Red),
                new VertexPositionColor(new Vector3(+0.5f, +0.5f, -0.5f), RgbaFloat.Green),
                new VertexPositionColor(new Vector3(+0.5f, +0.5f, +0.5f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector3(-0.5f, +0.5f, +0.5f), RgbaFloat.Yellow),
                // Bottom                                                             
                new VertexPositionColor(new Vector3(-0.5f,-0.5f, +0.5f),  RgbaFloat.Red),
                new VertexPositionColor(new Vector3(+0.5f,-0.5f, +0.5f),  RgbaFloat.Green),
                new VertexPositionColor(new Vector3(+0.5f,-0.5f, -0.5f),  RgbaFloat.Blue),
                new VertexPositionColor(new Vector3(-0.5f,-0.5f, -0.5f),  RgbaFloat.Yellow),
                // Left                                                               
                new VertexPositionColor(new Vector3(-0.5f, +0.5f, -0.5f), RgbaFloat.Red),
                new VertexPositionColor(new Vector3(-0.5f, +0.5f, +0.5f), RgbaFloat.Green),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, +0.5f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), RgbaFloat.Yellow),
                // Right                                                              
                new VertexPositionColor(new Vector3(+0.5f, +0.5f, +0.5f), RgbaFloat.Red),
                new VertexPositionColor(new Vector3(+0.5f, +0.5f, -0.5f), RgbaFloat.Green),
                new VertexPositionColor(new Vector3(+0.5f, -0.5f, -0.5f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector3(+0.5f, -0.5f, +0.5f), RgbaFloat.Yellow),
                // Back                                                               
                new VertexPositionColor(new Vector3(+0.5f, +0.5f, -0.5f), RgbaFloat.Red),
                new VertexPositionColor(new Vector3(-0.5f, +0.5f, -0.5f), RgbaFloat.Green),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector3(+0.5f, -0.5f, -0.5f), RgbaFloat.Yellow),
                // Front                                                              
                new VertexPositionColor(new Vector3(-0.5f, +0.5f, +0.5f), RgbaFloat.Red),
                new VertexPositionColor(new Vector3(+0.5f, +0.5f, +0.5f), RgbaFloat.Green),
                new VertexPositionColor(new Vector3(+0.5f, -0.5f, +0.5f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, +0.5f), RgbaFloat.Yellow),
            };

            ushort[] cubeIndices =
            {
                0,1,2, 0,2,3,
                4,5,6, 4,6,7,
                8,9,10, 8,10,11,
                12,13,14, 12,14,15,
                16,17,18, 16,18,19,
                20,21,22, 20,22,23,
            };

            _vertexBuffer = factory.CreateBuffer(new BufferDescription((uint)(vertices.Length * VertexPositionColor.SizeInBytes), BufferUsage.VertexBuffer));
            _indexBuffer = factory.CreateBuffer(new BufferDescription((uint)(cubeIndices.Length * 2), BufferUsage.IndexBuffer));

            _graphicsDevice.UpdateBuffer(_vertexBuffer, 0, vertices);
            _graphicsDevice.UpdateBuffer(_indexBuffer, 0, cubeIndices);

            VertexLayoutDescription vertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3),
                new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4));

            ShaderDescription vertexShaderDesc = new ShaderDescription(
                ShaderStages.Vertex,
                Encoding.UTF8.GetBytes(VertexCode),
                "main");

            ShaderDescription fragmentShaderDesc = new ShaderDescription(
                ShaderStages.Fragment,
                Encoding.UTF8.GetBytes(FragmentCode),
                "main");

            _shaders = factory.CreateFromSpirv(vertexShaderDesc, fragmentShaderDesc);

            GraphicsPipelineDescription pipelineDescription = new GraphicsPipelineDescription();
            pipelineDescription.BlendState = BlendStateDescription.SingleOverrideBlend;

            pipelineDescription.DepthStencilState = new DepthStencilStateDescription(
            depthTestEnabled: true,
            depthWriteEnabled: true,
            comparisonKind: ComparisonKind.LessEqual);

            pipelineDescription.RasterizerState = new RasterizerStateDescription(
            cullMode: FaceCullMode.Back,
            fillMode: PolygonFillMode.Solid,
            frontFace: FrontFace.Clockwise,
            depthClipEnabled: true,
            scissorTestEnabled: false);

            pipelineDescription.PrimitiveTopology = PrimitiveTopology.TriangleStrip;

            pipelineDescription.ResourceLayouts = System.Array.Empty<ResourceLayout>();

            pipelineDescription.ShaderSet = new ShaderSetDescription(
            vertexLayouts: new VertexLayoutDescription[] { vertexLayout },
            shaders: _shaders);

            pipelineDescription.Outputs = _graphicsDevice.SwapchainFramebuffer.OutputDescription;

            _pipeline = factory.CreateGraphicsPipeline(pipelineDescription);

            _commandList = factory.CreateCommandList();
        }

        private const string VertexCode = @"
        #version 450

        layout(location = 0) in vec3 Position;
        layout(location = 1) in vec4 Color;

        layout(location = 0) out vec4 fsin_Color;

        void main()
        {
            gl_Position = vec4(Position, 1);
            fsin_Color = Color;
        }";

        private const string FragmentCode = @"
        #version 450

        layout(location = 0) in vec4 fsin_Color;
        layout(location = 0) out vec4 fsout_Color;

        void main()
        {
            fsout_Color = fsin_Color;
        }";
    }

    struct VertexPositionColor
    {
        public Vector3 Position;
        public RgbaFloat Color;
        public VertexPositionColor(Vector3 position, RgbaFloat color)
        {
            Position = position;
            Color = color;
        }
        public const uint SizeInBytes = 28;
    }

    //struct VertexPositionColor
    //{
    //    public Vector2 Position; // This is the position, in normalized device coordinates.
    //    public RgbaFloat Color; // This is the color of the vertex.
    //    public VertexPositionColor(Vector2 position, RgbaFloat color)
    //    {
    //        Position = position;
    //        Color = color;
    //    }
    //    public const uint SizeInBytes = 24;
    //}
}