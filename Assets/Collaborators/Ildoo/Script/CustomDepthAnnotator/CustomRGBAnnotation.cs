using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Perception.GroundTruth;
using UnityEngine.Perception.GroundTruth.DataModel;
using UnityEngine.Perception.GroundTruth.Labelers;

public class CustomRGBAnnotation : Annotation
{
        /// <summary>
        /// The measurement strategy used to capture the depth image.
        /// </summary>
        public DepthMeasurementStrategy measurementStrategy { get; set; }

        /// <summary>
        /// The encoding format (png, exr, etc.) of the depth image.
        /// </summary>
        public ImageEncodingFormat imageFormat { get; set; }

        /// <summary>
        /// The range image's width and height in pixels.
        /// </summary>
        public Vector2 dimension { get; set; }

        /// <summary>
        /// The encoded range image data.
        /// </summary>
        public byte[] buffer { get; set; }

        /// <summary>
        /// Add image information about the depth image to message builder
        /// </summary>
        /// <param name="builder">The capture message to nest this annotation within.</param>
        public override void ToMessage(IMessageBuilder builder)
        {
            base.ToMessage(builder);
            builder.AddString("measurementStrategy", measurementStrategy.ToString());
            builder.AddString("imageFormat", imageFormat.ToString());
            builder.AddFloatArray("dimension", new[] { dimension.x, dimension.y });
            var key = $"{sensorId}.{annotationId}";
            builder.AddEncodedImage(key, "png", buffer);
        }

        /// <summary>
        /// Constructs a new <see cref="DepthAnnotation"/>.
        /// </summary>
        /// <param name="definition">The depth annotation definition.</param>
        /// <param name="sensorId">The sensor's string id.</param>
        /// <param name="measurementStrategy">The measurement strategy used to capture the depth image.</param>
        /// <param name="imageFormat">The encoding format of the depth image.</param>
        /// <param name="dimension">The width and height of the depth image in pixels.</param>
        /// <param name="buffer">The encoded range image data.</param>
        public CustomRGBAnnotation(
            CustomRGBDefinition definition,
            string sensorId,
            DepthMeasurementStrategy measurementStrategy,
            ImageEncodingFormat imageFormat,
            Vector2 dimension,
            byte[] buffer)
            : base(definition, sensorId)
        {
            this.measurementStrategy = measurementStrategy;
            this.imageFormat = imageFormat;
            this.dimension = dimension;
            this.buffer = buffer;
        }
}